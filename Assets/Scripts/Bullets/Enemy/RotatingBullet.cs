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
        private PooledBulletProduct _pooledProduct;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _pooledProduct = GetComponent<PooledBulletProduct>();
            _originalDirection = _rb.transform.up;
        }

        public void Initialized()
        {
            _rb.velocity = Vector3.zero;
            _rb.angularVelocity = 0;
            _existedTime = 0f;
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
                Deactivate();
        }

        public void Deactivate()
        {
            if (_pooledProduct != null)
            {
                _rb.velocity = Vector3.zero;
                _existedTime = 0f;
                _pooledProduct.Release();
            }
            else
                Destroy(gameObject);
        }
    }
}