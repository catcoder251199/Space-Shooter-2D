using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    namespace CrashChargerState
    {
        public class MoveToScreenState : IState
        {
            private CrashCharger _subject;
            private Vector3 _targetPosition;

            public MoveToScreenState(CrashCharger subject)
            {
                _subject = subject;
            }

            public void OnStateEnter()
            {
                _subject.transform.position = _subject.GetRandomOffScreenPosition();
                _targetPosition = _subject.GetRandomOnScreenPos();
                _subject.transform.rotation = Quaternion.LookRotation(Vector3.forward, _targetPosition - _subject.transform.position);
            }
            public void UpdateExecute() { }
            public void FixedUpdateExecute()
            {
                if (Vector2.Distance(_subject.Rigidbody.position, _targetPosition) > Mathf.Epsilon)
                {
                    Vector2 nextPosition = Vector2.MoveTowards(_subject.Rigidbody.position, _targetPosition, _subject.Speed * Time.fixedDeltaTime);
                    _subject.Rigidbody.MovePosition(nextPosition);
                }
                else
                {
                    _subject.ChangeState(_subject.WaiAndLookState);
                }
            }

            public void OnStateExit() { }
        }
    }
}