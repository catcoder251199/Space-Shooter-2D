using UnityEngine;

namespace Enemy
{
    namespace LaserShooterState
    {
        public class AttackState : IState
        {
            private LaserShooter _subject;
            private int _attackCount;
            private float _waitTime = 0f;

            public AttackState(LaserShooter subject)
            {
                _subject = subject;
            }

            public void OnStateEnter()
            {
                _attackCount = _subject.AttackCount;
                _waitTime = _subject.LaserGun.GetDelayTime();
                _subject.LaserGun.OnAttackFinishedEvent += OnOneAttackFinished;
                _subject.LaserGun.SwitchMode(LaserGunBase.Mode.Shoot);
                _subject.LaserGun.Activate();
                _attackCount--;
            }
            public void UpdateExecute() { }
            public void FixedUpdateExecute() 
            {
                if (_subject.Target.IsAlive() && _waitTime > 0)
                {
                    Vector3 directionToTarget = (Vector2)_subject.Target.transform.position - _subject.Rigidbody.position;
                    var targetRotation = Quaternion.LookRotation(Vector3.forward, directionToTarget);
                    var nextRotation = Quaternion.RotateTowards(_subject.transform.rotation, targetRotation, Time.fixedDeltaTime * _subject.RotateSpeed);
                    _subject.Rigidbody.MoveRotation(nextRotation);

                    _waitTime -= Time.fixedDeltaTime;
                }
            }

            public void OnStateExit() { }

            void OnOneAttackFinished()
            {

                if (_attackCount > 0)
                {
                    _waitTime = _subject.LaserGun.GetDelayTime();
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
