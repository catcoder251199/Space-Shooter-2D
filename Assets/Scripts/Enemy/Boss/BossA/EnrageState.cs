using System.Collections;
using UnityEngine;

namespace Enemy
{
    namespace BossA
    {
        public class EnrageState : MonoBehaviour, IState
        {
            [SerializeField] private BossA_StateMachine _subject;
            [SerializeField] private float _rushSpeed = 15f;
            [SerializeField] private float _moveToScreenSpeed = 5f;
            [SerializeField] private float _delayOnRush = 1.5f;
            [SerializeField] private float _waitTime = 1.5f;

            [SerializeField, Header("VFX")] private ParticleSystem _jumpEffect;
            [SerializeField] Transform _jumpEffectLocation;
            [SerializeField] string[] _enragedAnimationTriggerList;
            [SerializeField] Animator _animator;

            private float _waitedTime = 0f;
            private Vector2 _moveToScreenPos;
            private bool _updateEnabled = false;
            private enum State
            {
                None,
                Wait,
                Attack,
                MoveToScreen
            }

            private State _state;

            public void OnStateEnter()
            {
                Debug.Log("BossA.Enrage Enter");
                TriggerAnimation();
                _subject.SetShieldEnabled(true);
                _subject.MoveToNextEnrageThreshold();
                _subject.LookToTargetInstantly();
                _waitedTime = 0f;
                _updateEnabled = true;
                _state = State.Wait;
            }

            private void TriggerAnimation()
            {
                int animationIndex = Mathf.Clamp(_subject.GetEnragedCount(), 0, _enragedAnimationTriggerList.Length - 1);
                _animator.SetTrigger(_enragedAnimationTriggerList[animationIndex]);
            }

            private void ChangeState(State newState)
            {
                _state = newState;
            }

            public void UpdateExecute() { }

            public void FixedUpdateExecute()
            {
                if (!_updateEnabled)
                    return;

                switch (_state)
                {
                    case State.Wait:
                        WaitFixedUpdate();
                        break;
                    case State.Attack:
                        AttackFixedUpdate();
                        break;
                    case State.MoveToScreen:
                        MoveFixedUpdate();
                        break;
                }
            }

            private void MoveFixedUpdate()
            {
                float distance = Vector2.Distance(_subject.RigidBody.position, _moveToScreenPos);
                if (!Mathf.Approximately(distance, 0f))
                {
                    Vector2 nextPosition = Vector2.MoveTowards(_subject.RigidBody.position, _moveToScreenPos, _moveToScreenSpeed * Time.fixedDeltaTime);
                    _subject.RigidBody.MovePosition(nextPosition);
                    _subject.transform.rotation = Quaternion.Euler(0, 0, 180f);
                    //if (_subject.Target.IsAlive())
                    //    _subject.LookAtTarget();
                }
                else
                {
                    _subject.ChangeState(_subject.DecideState);
                }
            }

            private void AttackFixedUpdate() 
            {
                if (_subject.IsOffScreen())
                {
                    float topOffset = 5f;
                    // Determine the position at the top of the camera's view area as the starting point
                    Vector3 startPosition = Helper.Cam.WorldPos() + new Vector3(0, Helper.Cam.HalfHeight() + topOffset, 0); startPosition.z = 0;
                    _subject.transform.position = startPosition;
                    _moveToScreenPos = Helper.Cam.GetWorldPositionInViewSpace(0.5f, 0.8f);
                    ChangeState(State.MoveToScreen);
                }
            }

            private void WaitFixedUpdate()
            {
                if (_waitedTime < _waitTime)
                {
                    _subject.LookAtTarget();
                    _waitedTime += Time.deltaTime;
                }
                else
                {
                    StartCoroutine(RushToTarget());
                    ChangeState(State.Attack);
                }
            }
            private IEnumerator RushToTarget()
            {
                yield return new WaitForSeconds(_delayOnRush);
                if (_jumpEffect != null)
                {
                    var vfx = Instantiate(_jumpEffect, PlaySceneGlobal.Instance.VFXParent);
                    vfx.transform.position = _jumpEffectLocation.position;
                    vfx.transform.rotation = _jumpEffectLocation.rotation;
                }
                _subject.RigidBody.velocity = _subject.transform.up * _rushSpeed;
            }

           
            public void OnStateExit() {
                _subject.RigidBody.velocity = Vector2.zero;
                _subject.SetShieldEnabled(false);
                Debug.Log("BossA.Enrage Exit");
            }
        }

    }
}
