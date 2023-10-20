using Enemy;
using UnityEngine;

namespace Enemy
{
    namespace LaserShooterState
    {
        public class WaitState : IState
        {
            private LaserShooter _subject;
            private float _waitedTime = 0f;

            public WaitState(LaserShooter subject)
            {
                _subject = subject;
            }

            public void OnStateEnter()
            {
                _waitedTime = _subject.WaitTime;

                // Rotate to target
                var direction = _subject.Target.transform.position - _subject.transform.position; direction.z = 0f;
                _subject.transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
                _subject.LaserGun.SetSightLineEnabled(true);
            }
            public void UpdateExecute() { }
            public void FixedUpdateExecute() 
            {
                if (!_subject.Target.IsAlive())
                    return;

                if (_waitedTime > 0)
                {
                    Vector3 directionToTarget = (Vector2)_subject.Target.transform.position - _subject.Rigidbody.position;
                    var targetRotation = Quaternion.LookRotation(Vector3.forward, directionToTarget);
                    var nextRotation = Quaternion.RotateTowards(_subject.transform.rotation, targetRotation, Time.fixedDeltaTime * _subject.RotateSpeed);
                    _subject.Rigidbody.MoveRotation(nextRotation);
                    _waitedTime -= Time.fixedDeltaTime;
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
