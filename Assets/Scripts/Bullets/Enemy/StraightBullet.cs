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

        private PooledObject _pooledObject;
        public enum DestroyedCondition
        {
            LifeTime,
            OutOfScreen
        }
        public DestroyedCondition destroyedCondition;

        private Rigidbody2D _rb;
        private float _existedTime = 0f;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _pooledObject = GetComponent<PooledObject>();
        }

        private void Start() { }

        private void FixedUpdate()
        {
            if (destroyedCondition == DestroyedCondition.LifeTime)
            {
                if (_existedTime < lifeTime)
                    _existedTime += Time.fixedDeltaTime;

                if (_existedTime >= lifeTime)
                    Deactivate();
            }
            else if (destroyedCondition == DestroyedCondition.OutOfScreen)
            {
                if (!IsInScreen())
                    Deactivate();
            }

            _rb.velocity = transform.up * straightSpeed;
        }

        private bool IsInScreen()
        {
            return Helper.Cam.IsPositionInWorldCamRect(transform.position, 3f);
        }

        public void Deactivate()
        {
            if (_pooledObject != null)
            {
                _rb.velocity = Vector3.zero;
                _pooledObject.Release();
            }
            else
                Destroy(gameObject);
        }

        public void OnTriggerEnteredEventHappened(Collider2D collision)
        {
            // If the bullet hit something, then deactivate it
            if (destroyOnHit)
                Deactivate();
        }
    }
}