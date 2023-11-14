using System.Collections;
using UnityEngine;

namespace Enemy
{
    namespace CrashChargerState
    {
        public class ChargeState : IState
        {
            private CrashCharger _subject;
            private float _chargedTime = 0f;
            private float _delayStart = 0.5f;

            public ChargeState(CrashCharger subject)
            {
                _subject = subject;
            }

            public void OnStateEnter()
            {
                _chargedTime = _subject.ChargeTime + _delayStart;
                _subject.StartCoroutine(StartCharge());
            }

            private IEnumerator StartCharge()
            {
                yield return new WaitForSeconds(_delayStart);
                _subject.Rigidbody.velocity = _subject.transform.up * _subject.ChargeSpeed;
            }

            public void UpdateExecute() { }
            public void FixedUpdateExecute()
            {
                if (_subject.IsOutOfScreen())
                {
                    _subject.ChangeState(_subject.MoveToScreenState);
                    return;
                }

                if (_chargedTime > 0f)
                    _chargedTime -= Time.fixedDeltaTime;

                if (_chargedTime <= 0) // Object is on screen at the moment
                {

                    _subject.ChangeState(_subject.WaiAndLookState);
                }
            }
            
            public void OnStateExit() 
            {
                _subject.Rigidbody.velocity = Vector3.zero;
            }
        }
    }
}