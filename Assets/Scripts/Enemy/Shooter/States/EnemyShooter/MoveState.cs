using Enemy;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace Enemy
{
    namespace EnemyShooterState
    {
        public class MoveState : IState
        {
            private EnemyShooter _subject;
            private Vector3 _targetPosition;

            public MoveState(EnemyShooter subject)
            {
                _subject = subject;
            }

            public void OnStateEnter()
            {
                _targetPosition = Helper.Cam.GetRandomPositionInRect(0.1f, 0.9f, 0.8f, 0.5f);
            }
            public void UpdateExecute()
            {
                //if (Vector2.Distance(_subject.transform.position, _targetPosition) > Mathf.Epsilon)
                //{
                //    Vector2 moveDirection = _targetPosition - _subject.transform.position;
                //    _subject.transform.rotation = Quaternion.LookRotation(Vector3.forward, moveDirection);

                //    _subject.transform.position = Vector2.MoveTowards(_subject.transform.position, _targetPosition, _subject.Speed * Time.deltaTime);

                //}
                //else
                //{
                //    _subject.ChangeState(_subject.AttackState);
                //}
            }
            public void FixedUpdateExecute() 
            {
                if (Vector2.Distance(_subject.transform.position, _targetPosition) > Mathf.Epsilon)
                {
                    Vector2 nextPosition = Vector2.MoveTowards(_subject.Rigidbody.position, _targetPosition, _subject.Speed * Time.fixedDeltaTime);
                    _subject.Rigidbody.MovePosition(nextPosition);

                    Vector2 direction = (Vector2)_targetPosition - _subject.Rigidbody.position;
                    direction.Normalize();
                    float step = Time.fixedDeltaTime * _subject.RotateSpeed;
                    float toTargetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
                    float nextAngle = Mathf.MoveTowardsAngle(_subject.transform.eulerAngles.z, toTargetAngle, step);
                    _subject.Rigidbody.MoveRotation(nextAngle);
                }
                else
                {
                    _subject.ChangeState(_subject.AttackState);
                }
            }
            public void OnStateExit() { }
        }
    }
}
