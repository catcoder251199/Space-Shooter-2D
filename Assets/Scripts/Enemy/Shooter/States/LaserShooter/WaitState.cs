using Enemy;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.RuleTile.TilingRuleOutput;

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
                var targetRotation = Quaternion.LookRotation(Vector3.forward, direction);
                _subject.transform.rotation = Quaternion.RotateTowards(_subject.transform.rotation, targetRotation, _subject.RotateSpeed * Time.deltaTime);
               
                _subject.LaserGun.SwitchMode(SingleLaserGun.Mode.Probe);
            }
            public void UpdateExecute()
            {
                //if (_rotatedTime > 0)
                //{
                //    var direction = _subject.Target.transform.position - _subject.transform.position;
                //    direction.z = 0f;
                //    var targetRotation = Quaternion.LookRotation(Vector3.forward, direction);
                //    _subject.transform.rotation = Quaternion.RotateTowards(_subject.transform.rotation, targetRotation, _subject.RotateSpeed * Time.deltaTime);
                //    _rotatedTime -= Time.deltaTime;
                //}
                //else
                //{
                //    _subject.ChangeState(_subject.AttackState);
                //}
            }
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
