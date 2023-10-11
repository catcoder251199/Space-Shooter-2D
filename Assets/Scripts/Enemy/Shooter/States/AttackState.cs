using Enemy;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace Enemy
{
    namespace EnemyShooterState
    {
        public class AttackState : IState
        {
            private EnemyShooter _subject;
            private float _attackedTime;

            public AttackState(EnemyShooter subject)
            {
                _subject = subject;
            }

            public void OnStateEnter()
            {
                _attackedTime = _subject.AttackTime;
                _subject.ShootDevice.Activate();
            }
            public void Execute()
            {
                if (!_subject.Target.IsAlive())
                    return;

                if (_attackedTime > 0)
                {
                    var directionToTarget = _subject.Target.transform.position - _subject.transform.position;
                    directionToTarget.z = _subject.transform.position.z; // Keep the z coord of this object
                    var targetRotation = Quaternion.LookRotation(Vector3.forward, directionToTarget);
                    _subject.transform.rotation = Quaternion.RotateTowards(_subject.transform.rotation, targetRotation, Time.deltaTime * _subject.RotateSpeed);
                    _attackedTime -= Time.deltaTime;
                }
                else
                {
                    _subject.ChangeState(_subject.MoveState);
                }
            }
            public void OnStateExit()
            {
                _subject.ShootDevice.Deactivate();
            }
        }

    }
}
