using UnityEngine;
using Enemy.LaserShooterState;
namespace Enemy
{
    public partial class LaserShooter : FSMEnemy
    {
        [SerializeField, Header("Move State")] float _speed = 1f;

        [SerializeField, Header("Wait State")] float _waitTime = 3f;
        [SerializeField] float _rotateSpeed = 360f;

        [SerializeField, Header("Attack State")] float _delayBeforeAttack = 1f;
        [SerializeField] bool _followTargetOnDelay = false;
        [SerializeField] int _attackCount = 2;

        [SerializeField, Header("Other")] ParticleSystem _explosionEffect;
        [SerializeField] float _offsetFromBounds = 2f;
        [SerializeField] LaserGunBase _laserGun;
        private StartState _startState;
        private WaitState _waitState;
        private MoveState _moveState;
        private AttackState _attackState;

        private Player _target;
        private Rigidbody2D _rb;
        Health _health;

        // Properties Section
        public StartState StartState => _startState;
        public WaitState WaitState => _waitState;
        public AttackState AttackState => _attackState;
        public MoveState MoveState => _moveState;
        public LaserGunBase LaserGun => _laserGun;
        public float DelayBeforeAttack => _delayBeforeAttack;
        public Player Target => _target;
        public float Speed => _speed;
        public float RotateSpeed=> _rotateSpeed;
        public int AttackCount => _attackCount;
        public float WaitTime => _waitTime;
        public float OffsetFromBounds => _offsetFromBounds;
        public bool FollowTargetOnDelay => _followTargetOnDelay;
        public Rigidbody2D Rigidbody => _rb;

        private void Awake()
        {
            _startState = new StartState(this);
            _waitState = new WaitState(this);
            _attackState = new AttackState(this);
            _moveState = new MoveState(this);

            _rb = GetComponent<Rigidbody2D>();
            _health = GetComponent<Health>();
        }

        private void Start()
        {
            _target = GameManager.Instance.Player;
            if (_target == null)
                Debug.LogError("LaserShooter.Start(): _target == null");

            ChangeState(_startState);
        }
        private void Update()
        {
            _currentState?.UpdateExecute();
        }
        private void FixedUpdate()
        {
            _currentState?.FixedUpdateExecute();
        }

        public void OnTriggerEnter2D(Collider2D collision)
        {
            DamagableCollider hitCollider = collision.GetComponent<DamagableCollider>();
            if (hitCollider != null)
            {
                if (hitCollider.CompareTag(PlaySceneGlobal.Instance.Tag_PlayerBullet))
                {
                    var bullet = hitCollider.GetComponent<BulletBase>();
                    if (bullet != null)
                        bullet.TriggerHitVFX();
                    bool isCritical = false;
                    int damage = hitCollider.GetCalculatedDamage(out isCritical);
                    TakeDamage(damage, isCritical);
                    Destroy(hitCollider.gameObject);
                }
            }
        }
        private void TakeDamage(int damage, bool isCritical = false)
        {
            _health.SetHealth(_health.GetHealth() - Mathf.Max(0, damage));
            DamagePopup.Create(damage, transform.position, isCritical);
            if (_health.GetHealth() <= 0)
            {
                OnDied();
            }
        }

        private void OnDied()
        {
            if (_explosionEffect != null)
                Instantiate(_explosionEffect, transform.position, Quaternion.identity, PlaySceneGlobal.Instance.VFXParent);

            Destroy(gameObject, 0.1f);
        }
    }
}


