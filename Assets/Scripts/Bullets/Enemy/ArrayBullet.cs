using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Enemy
{
    // Array of STRAIGHT BULLETS
    public class ArrayBullet : EnemyBulletBase
    {
        [SerializeField] private StraightBullet _toCloneBullet;
        [SerializeField] private bool _initOnStart = false;
        private PooledBulletProduct _pooledProduct;

        public float Count = 0;
        public float Gap = 0;
        public float StraightSpeed = 10f;
        public float LifeTime = 3f;

        private Rigidbody2D _rb;
        private float _existedTime = 0f;
        private List<PooledBulletProduct> _clonedList = new List<PooledBulletProduct>();

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _pooledProduct = GetComponent<PooledBulletProduct>();
        }

        private void Start()
        {
            if (_initOnStart)
                Initialize();
        }

        public void Initialize()
        {
            float yPos = 0f;
            _existedTime = 0f;
            for (int i = 0; i < Count; i++)
            {
                //var clone = Instantiate(_toCloneBullet, transform);
                var clone = BulletFactory.Instance.CreateBulletProduct(_toCloneBullet.gameObject.GetInstanceID());
                _clonedList.Add(clone);
                var bullet = clone?.GetComponent<StraightBullet>();
                if (bullet != null)
                {
                    bullet.transform.parent = transform;
                    yPos -= Gap;
                    bullet.transform.localPosition = new Vector3(0, yPos, 0);
                    bullet.destroyedCondition = StraightBullet.DestroyedCondition.None;
                    bullet.straightSpeed = 0;
                }
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
                Deactivate();
        }

        private void Deactivate()
        {
            if (_pooledProduct != null)
            {
                foreach (var pooledBullet in _clonedList)
                    pooledBullet.Release();
                _clonedList.Clear();
                _pooledProduct.Release();
                _existedTime = 0f;
            }
            else
                Destroy(gameObject);
        }
    }
}