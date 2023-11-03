using System.Collections;
using UnityEngine;

namespace Enemy
{
    public class OrientatedToTargetBullet : EnemyBulletBase
    {
        public float StraightSpeed = 10f;
        public float OrientationTime = 2f;
        public float LifeTime = 3f;
        public float ToTargetSpeed = 3f;
        public float RotateSpeed = 1f;
        public Transform Target; 

        private Rigidbody2D _rb;
        private float _existedTime = 0f;
        private float _orientatedTime = 0f;

        private Vector2 _originalDirection;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            StartCoroutine(StartRoutine());
        }

        private IEnumerator StartRoutine()
        {
            float delay = 3f;
            _existedTime = -delay;
            yield return new WaitForSeconds(delay);
            _originalDirection = transform.up;

            Vector2 u = _originalDirection;
            Vector2 v = Target.position - _rb.transform.position;
            Vector2 u_On_v_Projection = (Vector2.Dot(u, v) / v.magnitude) * v.normalized;
            Vector2 orthogonal_to_v_Vector = u_On_v_Projection - u;
            Vector2 additionalVelocity = orthogonal_to_v_Vector.normalized * ToTargetSpeed;

            _rb.velocity = _originalDirection * StraightSpeed + additionalVelocity;
            _rb.angularVelocity = Mathf.Sign(Vector2.SignedAngle(u, v)) * RotateSpeed;
        }

        private void FixedUpdate()
        {
            if (_existedTime < LifeTime)
            {
                Vector2 toTargetDirection = Vector2.zero;
                if (_orientatedTime < OrientationTime)
                {
                    if (Target != null)
                        toTargetDirection = (Target.position - _rb.transform.position).normalized;


                    //float step = Time.fixedDeltaTime * RotateSpeed;
                    //float toTargetAngle = Mathf.Atan2(toTargetDirection.y, toTargetDirection.x) * Mathf.Rad2Deg - 90;
                    //float nextAngle = Mathf.MoveTowardsAngle(transform.eulerAngles.z, toTargetAngle, step);
                    //_rb.MoveRotation(nextAngle);
                    _orientatedTime += Time.fixedDeltaTime;
                }
                else
                {
                    _originalDirection = _rb.transform.up;
                }
                _existedTime += Time.fixedDeltaTime;
            }
            else
            {
                Debug.Log("OrientatedBullet is destroyed");
                Destroy(gameObject);
            }
        }

    }
}