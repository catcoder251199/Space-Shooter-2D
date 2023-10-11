using System.Collections;
using System.Collections.Generic;
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

        private void Awake()
        {
            _startState = new StartState(this);
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


