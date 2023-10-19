using UnityEngine;
using Enemy.LaserShooterState;
namespace Enemy
{
    public partial class LaserShooter : FSMEnemy
    {
        [SerializeField] float _speed = 1f;
        [SerializeField] float _rotateSpeed = 360f;
        [SerializeField] int _attackCount = 2;
        [SerializeField] float _waitTime = 3f;
        [SerializeField] float _offsetFromBounds = 2f;
        [SerializeField] LaserGunBase _laserGun;
        public float TestTime = 0f;
        private StartState _startState;
        private WaitState _waitState;
        private MoveState _moveState;
        private AttackState _attackState;

        private Player _target;
        private Rigidbody2D _rb;

        // Properties Section
        public StartState StartState => _startState;
        public WaitState WaitState => _waitState;
        public AttackState AttackState => _attackState;
        public MoveState MoveState => _moveState;
        public LaserGunBase LaserGun => _laserGun;

        public Player Target => _target;
        public float Speed => _speed;
        public float RotateSpeed=> _rotateSpeed;
        public int AttackCount => _attackCount;
        public float WaitTime => _waitTime;
        public float OffsetFromBounds => _offsetFromBounds;
        public Rigidbody2D Rigidbody => _rb;

        private void Awake()
        {
            _startState = new StartState(this);
            _waitState = new WaitState(this);
            _attackState = new AttackState(this);
            _moveState = new MoveState(this);

            _rb = GetComponent<Rigidbody2D>();
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
    }
}


