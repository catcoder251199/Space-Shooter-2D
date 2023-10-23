using System.Collections;
using UnityEngine;

namespace Enemy
{
    public class StraightBullet : EnemyBulletBase
    {
        public float StraightSpeed = 10f;
        public float LifeTime = 3f;

        private Rigidbody2D _rb;
        private DamagableCollider _damageableCollider;
        private float _existedTime = 0f;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _damageableCollider = GetComponent<DamagableCollider>();
        }

        private void Start() { }

        private void FixedUpdate()
        {
            if (_existedTime < LifeTime)
            {
                _rb.velocity = transform.up * StraightSpeed;
                _existedTime += Time.fixedDeltaTime;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}