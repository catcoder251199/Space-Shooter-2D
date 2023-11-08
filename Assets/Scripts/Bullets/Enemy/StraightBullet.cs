using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Enemy
{
    public class StraightBullet : EnemyBulletBase
    {
        public float straightSpeed = 10f;
        public float lifeTime = 3f;

        [Tooltip("whether the bullet should be deactivated in object pool when hitting something")]
        public bool destroyOnHit = true;
        [SerializeField] private ParticleSystem _hitEffect;

        private PooledBulletProduct _pooledProduct;
        public enum DestroyedCondition
        {
            None,
            LifeTime,
            OutOfScreen
        }
        public DestroyedCondition destroyedCondition;

        private Rigidbody2D _rb;
        private float _existedTime = 0f;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _pooledProduct = GetComponent<PooledBulletProduct>();
        }

        public void Initialized()
        {
            _rb.velocity = Vector3.zero;
            _rb.angularVelocity = 0;
            _existedTime = 0f;
            transform.position = Vector3.zero;
        }

        private void Start() { }

        private void Update()
        {
            if (destroyedCondition == DestroyedCondition.LifeTime)
            {
                if (_existedTime < lifeTime)
                    _existedTime += Time.deltaTime;

                if (_existedTime >= lifeTime)
                    Deactivate();
            }
            else if (destroyedCondition == DestroyedCondition.OutOfScreen)
            {
                if (!IsInScreen())
                    Deactivate();
            }
        }

        private void FixedUpdate()
        {
            _rb.velocity = transform.up * straightSpeed;
        }

        private bool IsInScreen()
        {
            return Helper.Cam.IsPositionInWorldCamRect(transform.position, 3f);
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

        public void OnTriggerEnteredEventHappened(Collider2D collision)
        {
            // If the bullet hit something, then deactivate it
            if (destroyOnHit)
            {
                Instantiate(_hitEffect, transform.position, Quaternion.identity, PlaySceneGlobal.Instance.VFXParent);
                Deactivate();
            }
        }
    }
}