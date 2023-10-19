using UnityEngine;

using Enemy.EnemyShooterState;
namespace Enemy
{
    public partial class EnemyShooter : FSMEnemy
    {
        [SerializeField] float _speed = 1f;
        [SerializeField] float _rotateSpeed = 1f;
        [SerializeField] float _attackTime = 1f;
        [SerializeField] float _offsetFromBounds = 2f;
        [SerializeField] AutoShootDevice _shootDevice;
        [SerializeField] ParticleSystem _explosionEffect;

        private Rigidbody2D _rb;
        private Health _health;

        private StartState _startState;
        private MoveState _moveState;
        private AttackState _attackState;

        private Player _target;

        // Properties Section
        public StartState StartState => _startState;
        public AttackState AttackState => _attackState;
        public MoveState MoveState => _moveState;
        public AutoShootDevice ShootDevice => _shootDevice;

        public Player Target => _target;
        public float Speed => _speed;
        public float RotateSpeed=> _rotateSpeed;
        public float AttackTime => _attackTime;
        public float OffsetFromBounds => _offsetFromBounds;
        public Rigidbody2D Rigidbody => _rb;

        private void Awake()
        {
            _startState = new StartState(this);
            _attackState = new AttackState(this);
            _moveState = new MoveState(this);

            ChangeState(_startState);
            _target = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
            _rb = GetComponent<Rigidbody2D>();
            _health = GetComponent<Health>();
        }

        private void Update()
        {
            _currentState.UpdateExecute();
        }

        private void FixedUpdate()
        {
            _currentState.FixedUpdateExecute();
        }

        public void OnTriggerEnter2D(Collider2D collision)
        {
            DamagableCollider hitCollider = collision.GetComponent<DamagableCollider>();
            if (hitCollider != null)
            {
                //Debug.Log("Enemy take damage " + hitCollider.GetDamage());
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
        private void TakeDamage(int damage, bool isCritical)
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


