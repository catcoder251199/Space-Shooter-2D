using Enemy;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

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
            }
            public void UpdateExecute()
            {
                if (Vector2.Distance(_subject.transform.position, _targetPosition) > Mathf.Epsilon)
                {
                    Vector2 moveDirection = _targetPosition - _subject.transform.position;
                    _subject.transform.rotation = Quaternion.LookRotation(Vector3.forward, moveDirection);

                    _subject.transform.position = Vector2.MoveTowards(_subject.transform.position, _targetPosition, _subject.Speed * Time.deltaTime);

                }
                else
                {
                    _subject.ChangeState(_subject.WaitState);
                }
            }
            public void FixedUpdateExecute() { }
            public void OnStateExit() { }
        }
    }
}
