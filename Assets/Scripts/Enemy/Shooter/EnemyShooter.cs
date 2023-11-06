using UnityEngine;
using UnityEngine.Pool;

using Enemy.EnemyShooterState;
namespace Enemy
{
    public class EnemyShooter : FSMEnemy
    {
        [SerializeField, Tooltip("if true, the shooter start to play on the first active frame. If false, the shooter don't do anything. Set it false when this shooter should be spawned/enabled by something else")] 
        bool _playOnStart = false;

        [SerializeField] float _speed = 1f;
        [SerializeField] float _rotateSpeed = 1f;
        [SerializeField] float _attackTime = 1f;
        [SerializeField] float _offsetFromBounds = 2f;
        [SerializeField] AutoShootDevice _shootDevice;
        [SerializeField] ParticleSystem _explosionEffect;

        [SerializeField, Header("Pooled Spawnable Object/Product")] private PooledSpawnableProduct _pooledProduct;

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
        public float RotateSpeed => _rotateSpeed;
        public float AttackTime => _attackTime;
        public float OffsetFromBounds => _offsetFromBounds;
        public Rigidbody2D Rigidbody => _rb;

        private void Awake()
        {
            _startState = new StartState(this);
            _attackState = new AttackState(this);
            _moveState = new MoveState(this);

            _rb = GetComponent<Rigidbody2D>();
            _health = GetComponent<Health>();
        }

        void Start()
        {
            if (_playOnStart)
                Initialize();
        }

        private void Update()
        {
            if(_currentState != null)
                _currentState.UpdateExecute();
        }

        private void FixedUpdate()
        {
            if (_currentState != null)
                _currentState.FixedUpdateExecute();
        }

        public void OnTakeDamage(int damage, bool isCritical = false)
        {
            DamagePopup.Create(damage, transform.position, isCritical);
            if (_health.GetHealth() <= 0)
                OnDied();
        }

        private void OnDied()
        {
            if (_explosionEffect != null)
                Instantiate(_explosionEffect, transform.position, Quaternion.identity, PlaySceneGlobal.Instance.VFXParent);
            Deactivate();
        }

        public void Initialize()
        {
            _target = GameManager.Instance.Player;
            if (_target == null)
                Debug.LogError("SelfDestructor.Start(): _target == null");

            _health.SetHealth(_health.GetMaxHealth());
            ChangeState(_startState);
        }

        public void Deactivate()
        {
            if (_pooledProduct != null)
            {
                _rb.velocity = Vector3.zero;
                _rb.angularVelocity = 0;
                transform.position = Vector3.zero;
                transform.rotation = Quaternion.identity;
                _pooledProduct.Release();
            }
            else
                Destroy(gameObject);
        }

    }
}


