using System.Collections;
using UnityEngine;

namespace Enemy
{
    // Array of STRAIGHT BULLETS
    public class ArrayBullet : EnemyBulletBase
    {
        [SerializeField] private StraightBullet _toCloneBullet;

        public float Count = 0;
        public float Gap = 0;
        public float StraightSpeed = 10f;
        public float LifeTime = 3f;

        private Rigidbody2D _rb;
        private float _existedTime = 0f;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }
        private void Start()
        {
            float yPos = 0f;
            for (int i = 0; i < Count; i++)
            {
                var clone = Instantiate(_toCloneBullet, transform);
                yPos -= Gap;
                clone.transform.localPosition = new Vector3(0, yPos, 0);
                clone.lifeTime = LifeTime;
                clone.straightSpeed = StraightSpeed;
            }
        }

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