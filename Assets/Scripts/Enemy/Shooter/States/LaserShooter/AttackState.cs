using Enemy;
using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace Enemy
{
    namespace LaserShooterState
    {
        public class AttackState : IState
        {
            private LaserShooter _subject;
            private int _attackCount;

            public AttackState(LaserShooter subject)
            {
                _subject = subject;
            }

            public void OnStateEnter()
            {
                _attackCount = _subject.AttackCount;
                _subject.LaserGun.OnAttackFinishedEvent += OnOneAttackFinished;
                _subject.LaserGun.SwitchMode(SingleLaserGun.Mode.Shoot);
                _subject.LaserGun.Activate();
                _attackCount--;
            }
            public void Execute()
            {
                
            }
            public void OnStateExit()
            {
            }

            void OnOneAttackFinished()
            {
                if (_attackCount > 0)
                {
                    _subject.LaserGun.Activate();
                    _attackCount--;
                }
                else
                {
                    _subject.LaserGun.OnAttackFinishedEvent -= OnOneAttackFinished; //unsubscribe event once we've run out of number of attacks
                    _subject.ChangeState(_subject.MoveState);
                }
            }
        }
    }
}
