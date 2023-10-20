using Enemy;
using UnityEngine;

namespace Enemy
{
    namespace LaserShooterState
    {
        public class MoveState : IState
        {
            private LaserShooter _subject;
            private Vector3 _targetPosition;

            public MoveState(LaserShooter subject)
            {
                _subject = subject;
            }

            public void OnStateEnter()
            {
                _targetPosition = Helper.Cam.GetRandomPositionInRect(0.1f, 0.9f, 0.8f, 0.5f);
                _subject.LaserGun.SetSightLineEnabled(false);
            }
            public void UpdateExecute()
            {
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
                    _subject.ChangeState(_subject.WaitState);
                }
            }
            public void OnStateExit() { }
        }
    }
}
