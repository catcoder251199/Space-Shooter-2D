using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Enemy
{
    public class StraightBullet : EnemyBulletBase
    {
        public float StraightSpeed = 10f;
        public float LifeTime = 3f;
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
        }

        private void Start() { }

        private void FixedUpdate()
        {
            if (destroyedCondition == DestroyedCondition.LifeTime)
            {
                if (_existedTime < LifeTime)
                {
                    _existedTime += Time.fixedDeltaTime;
                }

                if (_existedTime >= LifeTime)
                    Destroy(gameObject);
            }
            else if (destroyedCondition == DestroyedCondition.OutOfScreen)
            {
                if (!IsInScreen())
                    Destroy(gameObject);
            }

            _rb.velocity = transform.up * StraightSpeed;
        }

        private bool IsInScreen()
        {
            return Helper.Cam.IsPositionInWorldCamRect(transform.position, 3f);
        }
    }
}