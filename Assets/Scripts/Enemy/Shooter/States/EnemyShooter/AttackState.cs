using UnityEngine;

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
                _attackedTime = _subject.AttackTime + _subject.ShootDevice.GetDelayStart();
                _subject.ShootDevice.Activate();
            }
            public void UpdateExecute()
            {}
            public void FixedUpdateExecute() 
            {
                if (!_subject.Target.IsAlive())
                {
                    if (_subject.ShootDevice.IsActivate())
                        _subject.ShootDevice.Deactivate();
                    return;
                }
                else
                {
                    if (!_subject.ShootDevice.IsActivate())
                        _subject.ShootDevice.Activate();
                }

                if (_attackedTime > 0)
                {
                    RotateTo2D(_subject.Target.transform, _subject.RotateSpeed);
                    _attackedTime -= Time.fixedDeltaTime;
                }
                else
                {
                    _subject.ChangeState(_subject.MoveState);
                }
            }
            private void RotateTo2D(Transform target, float rotateSpeed)
            {
                var directionToTarget = target.transform.position - _subject.transform.position;
                float targetAngle = Vector2.SignedAngle(_subject.transform.up, directionToTarget);
                float angleDirection = Mathf.Sign(targetAngle);
                float nextAngle = _subject.Rigidbody.rotation + angleDirection * rotateSpeed * Time.fixedDeltaTime;
                if (_subject.Rigidbody.rotation < _subject.Rigidbody.rotation + targetAngle)
                {
                    nextAngle = Mathf.Clamp(nextAngle,
                    _subject.Rigidbody.rotation,
                    _subject.Rigidbody.rotation + targetAngle);
                }
                else
                {
                    nextAngle = Mathf.Clamp(nextAngle,
                        _subject.Rigidbody.rotation + targetAngle,
                   _subject.Rigidbody.rotation
                   );
                }
                _subject.Rigidbody.MoveRotation(nextAngle);
            }
            public void OnStateExit()
            {
                _subject.ShootDevice.Deactivate();
            }
        }

    }
}
