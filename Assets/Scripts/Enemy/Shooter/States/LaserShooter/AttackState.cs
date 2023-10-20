using UnityEngine;

namespace Enemy
{
    namespace LaserShooterState
    {
        public class AttackState : IState
        {
            private LaserShooter _subject;
            private int _attackCount;
            private float _waitTime = 1f;

            public AttackState(LaserShooter subject)
            {
                _subject = subject;
            }

            public void OnStateEnter()
            {
                _attackCount = _subject.AttackCount;
                if(_subject.FollowTargetOnDelay)
                    _waitTime = _subject.DelayBeforeAttack;
                _subject.LaserGun.SetSightLineEnabled(true);
                _subject.LaserGun.ActivateLaserBeam(_subject.DelayBeforeAttack, true, OnOneAttackFinished);
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
                    if (_subject.FollowTargetOnDelay)
                        _waitTime = _subject.DelayBeforeAttack;
                    _subject.LaserGun.SetSightLineEnabled(true);
                    _subject.LaserGun.ActivateLaserBeam(_subject.DelayBeforeAttack, true, OnOneAttackFinished);
                    _attackCount--;
                }
                else
                    _subject.ChangeState(_subject.MoveState);
            }
        }
    }
}
