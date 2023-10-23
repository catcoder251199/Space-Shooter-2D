using System.Collections;
using UnityEngine;

namespace Enemy
{
    public class RotatingBullet : EnemyBulletBase
    {
        public float RotateSpeed = 10f;
        public float StraightSpeed = 10f;
        public bool ClockwiseRotate = true;
        public float LifeTime = 3f;

        private Rigidbody2D _rb;
        private float _existedTime = 0f;
        private Vector2 _originalDirection;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _originalDirection = _rb.transform.up;
        }

        private void Start() { }

        private void FixedUpdate()
        {
            if (_existedTime < LifeTime)
            {
                _rb.angularVelocity = ClockwiseRotate ? -RotateSpeed : RotateSpeed;
                _rb.velocity = StraightSpeed * _originalDirection;
                _existedTime += Time.fixedDeltaTime;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}