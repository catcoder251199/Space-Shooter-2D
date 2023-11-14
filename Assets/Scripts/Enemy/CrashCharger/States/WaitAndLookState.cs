using UnityEngine;

namespace Enemy
{
    namespace CrashChargerState
    {
        public class WaitAndLookState : IState
        {
            private CrashCharger _subject;
            private float _waitedTime;

            public WaitAndLookState(CrashCharger subject)
            {
                _subject = subject;
            }

            public void OnStateEnter()
            {
                _subject.Rigidbody.velocity = Vector3.zero;
                _waitedTime = _subject.WaitTime;
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
                    _subject.ChangeState(_subject.ChargeState);
                }
            }

            public void OnStateExit() { }
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
        }
    }
}