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
        [SerializeField] SingleLaserGun _laserGun;
        public float TestTime = 0f;
        private StartState _startState;
        private WaitState _waitState;
        private MoveState _moveState;
        private AttackState _attackState;

        private Player _target;

        // Properties Section
        public StartState StartState => _startState;
        public WaitState WaitState => _waitState;
        public AttackState AttackState => _attackState;
        public MoveState MoveState => _moveState;
        public SingleLaserGun LaserGun => _laserGun;

        public Player Target => _target;
        public float Speed => _speed;
        public float RotateSpeed=> _rotateSpeed;
        public int AttackCount => _attackCount;
        public float WaitTime => _waitTime;
        public float OffsetFromBounds => _offsetFromBounds;

        private void Awake()
        {
            _startState = new StartState(this);
            _waitState = new WaitState(this);
            _attackState = new AttackState(this);
            _moveState = new MoveState(this);

            ChangeState(_startState);
            _target = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        }

        private void Update()
        {
            _currentState.Execute();
        }
    }
}


