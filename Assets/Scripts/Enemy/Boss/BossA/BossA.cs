using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy.BossA;
using Helper;
using TMPro;

namespace Enemy
{
    namespace BossA 
    {
        public class BossA_StateMachine : FSMEnemy
        {
            [SerializeField] private EnemyShield _shield;
            [SerializeField] float _rotateSpeed = 30f;
            [SerializeField] ParticleSystem _explosionEffect;
            [SerializeField] int[] _enrageHpThreshHold;
            private int _currentEnrageThresholdIdx;

            private Player _target;
            private StartState _startState;
            private AttackStateA _attackStateA;
            private AttackStateB _attackStateB;
            private AttackStateC _attackStateC;
            private MoveState _moveState;
            private DecideState _decideState;
            private EnrageState _enrageState;

            public StartState StartState => _startState;
            public AttackStateA AttackStateA => _attackStateA;
            public AttackStateB AttackStateB => _attackStateB;
            public AttackStateC AttackStateC => _attackStateC;
            public DecideState DecideState => _decideState;
            public EnrageState EnrageState => _enrageState;
            public MoveState MoveState => _moveState;
            public Rigidbody2D RigidBody => _rb;
            public Player Target => _target;
            public EnemyShield Shield => _shield;
            public bool Damageable { get; set; }

            private IState[] _attackStates;
            private int _currenAttackState;
            private Rigidbody2D _rb;
            private Health _health;

            void Awake()
            {
                _rb = GetComponent<Rigidbody2D>();
                _health = GetComponent<Health>();

                _startState = GetComponent<StartState>();
                _attackStateA = GetComponent<AttackStateA>();
                _attackStateB = GetComponent<AttackStateB>();
                _attackStateC = GetComponent<AttackStateC>();
                _attackStates = new IState[] { _attackStateA, _attackStateB, _attackStateC };
                _currenAttackState = -1;

                _decideState = GetComponent<DecideState>();
                _moveState = GetComponent<MoveState>();
                _enrageState = GetComponent<EnrageState>();

                Damageable = true;
            }

            void Start()
            {
                _target = GameManager.Instance.Player;
                if (_target == null)
                    Debug.LogError("SelfDestructor.Start(): _target == null");

                ChangeState(_startState);
            }

            public int GetEnragedCount()
            {
                return _currentEnrageThresholdIdx;
            }

            private int GetEnrageHpThreshHold()
            {
                _currentEnrageThresholdIdx = Mathf.Clamp(_currentEnrageThresholdIdx, 0, _enrageHpThreshHold.Length -1);
                return _enrageHpThreshHold[_currentEnrageThresholdIdx];
            }

            private bool IsEnraged()
            {
                bool outOfBounds = _currentEnrageThresholdIdx >= _enrageHpThreshHold.Length;
                if (outOfBounds)
                    return false;

                int enragedThreshold = GetEnrageHpThreshHold();
                return _health.GetHealth() < enragedThreshold;
            }

            public void MoveToNextEnrageThreshold()
            {
                _currentEnrageThresholdIdx++;
            }

            void Update()
            {
                if (!_target.IsAlive())
                    return;
                _currentState.UpdateExecute();
            }

            private bool IsAlive()
            {
                return _health.GetHealth() > 0;
            }

            private void FixedUpdate()
            {
                if (!_target.IsAlive())
                    return;
                _currentState.FixedUpdateExecute();
            }

            public void LookAtTarget()
            {
                if (_target == null)
                    return;

                Vector2 direction = _target.transform.position - transform.position;
                float step = Time.fixedDeltaTime * _rotateSpeed;
                float toTargetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
                float nextAngle = Mathf.MoveTowardsAngle(transform.eulerAngles.z, toTargetAngle, step);
                _rb.MoveRotation(nextAngle);
            }

            public void ChangeToNextAttackState()
            {
                _currenAttackState = (++_currenAttackState) % _attackStates.Length;
                ChangeState(_attackStates[_currenAttackState]);
            }

            public void SetShieldEnabled(bool enabled)
            {
                _shield.gameObject.SetActive(enabled);
                Damageable = enabled;
            }
            public override void ChangeState(IState _nextState)
            {
                base.ChangeState(_nextState);
            }
           
            public void OnTakeDamage(int damage, bool isCritical)
            {
                //_health.SetHealth(_health.GetHealth() - Mathf.Max(0, damage));
                DamagePopup.Create(damage, transform.position, isCritical);
                if (_health.GetHealth() <= 0)
                {
                    OnDied();
                }
                else if (IsEnraged())
                {
                    Debug.Log("Is Enraged after taken damage");
                    ChangeState(_enrageState);
                }
            }

            private void OnDied()
            {
                if (_explosionEffect != null) 
                {
                    var vfx = Instantiate(_explosionEffect, transform.position, Quaternion.identity, PlaySceneGlobal.Instance.VFXParent);
                    vfx.transform.localScale = Vector3.zero * 5;
                }

                if (_explosionEffect != null)
                {
                    Instantiate(_explosionEffect, transform.position, Quaternion.identity, PlaySceneGlobal.Instance.VFXParent);
                }
                Destroy(gameObject, 0.1f);
            }

            public void LookToTargetInstantly()
            {
                // Rotate to target
                var direction = Target.transform.position - transform.position; direction.z = 0f;
                transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
            }
            public bool IsOffScreen()
            {
                return !Cam.IsPositionInWorldCamRect(transform.position, 4);
            }
        }
    }
}
