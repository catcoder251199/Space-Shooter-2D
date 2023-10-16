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
            private float _rotatedTime = 0f;

            public WaitState(LaserShooter subject)
            {
                _subject = subject;
            }

            public void OnStateEnter()
            {
                _rotatedTime = _subject.WaitTime;
                _subject.LaserGun.SwitchMode(SingleLaserGun.Mode.Probe);
            }
            public void Execute()
            {
                if (_rotatedTime > 0)
                {
                    var direction = _subject.Target.transform.position - _subject.transform.position;
                    direction.z = 0f;
                    var targetRotation = Quaternion.LookRotation(Vector3.forward, direction);
                    _subject.transform.rotation = Quaternion.RotateTowards(_subject.transform.rotation, targetRotation, _subject.RotateSpeed * Time.deltaTime);
                    _rotatedTime -= Time.deltaTime;
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
