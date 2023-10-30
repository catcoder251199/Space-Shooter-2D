using Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private bool _playOnStart = false;
    [SerializeField] float _offsetFromBounds = 2f; // in pixels

    // Straight Speed
    [SerializeField] private float _floatingSpeedMin = 1f;
    [SerializeField] private float _floatingSpeedMax = 1.5f;
    private float _floatingSpeed = 1f;
    // Scale
    [SerializeField] private float _minScaleXY = 1f;
    [SerializeField] private float _maxScaleXY = 1.5f;
    // Rotate Speed
    [SerializeField] private float _minRotateSpeed = 30;
    [SerializeField] private float _maxRotateSpeed = 60;
    private float _rotateSpeed = 360f;
    // Visual
    [SerializeField] private GameObject[] _meteorVisuals;
    private GameObject _visual;
    [SerializeField] ParticleSystem _explosionEffect;

    //// Hit
    //[SerializeField, Header("Hit")] float _hitRate = 1f;
    //private float _hitNextTime = -1f;
    //[SerializeField] private int _damagePerHit = 10;

    // Object Pool
    [SerializeField, Header("Pooled Spawnable Object/Product")] private PooledSpawnableProduct _pooledProduct;

    //private Vector2 _moveDir;
    private Rigidbody2D _rb;
    //private Vector2 _targetPosition;
    //private float _estimatedFlyToTime = -1f;
    //private float _floatingTime = 0f;
    private Health _health;

    private Vector2 _prePosition;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _health = GetComponent<Health>();
    }

    private void Start()
    {
        if (_playOnStart)
        {
            Initialize();
        }
    }
    public void Initialize()
    {
        // Setup visual
        if (_visual == null)
            _visual = Instantiate(_meteorVisuals[Random.Range(0, _meteorVisuals.Length - 1)], Vector3.zero, Quaternion.identity, this.transform);
        
        // Setup Scale
        float randomScale = Random.Range(_minScaleXY, _maxScaleXY);
        transform.localScale = new Vector3(randomScale, randomScale, 1f);

        // Setup speed
        _floatingSpeed = Random.Range(_floatingSpeedMin, _floatingSpeedMax);
        _rotateSpeed = Random.Range(_minRotateSpeed, _maxRotateSpeed);

        // Setup startPosition
        bool startFromLeft = Random.value > 0.5f;
        Vector2 startPosition;
        Vector2 endPosition;
        if (startFromLeft)
        {
            startPosition = Helper.Cam.GetLeftSideRandomPos(_offsetFromBounds, 0f, 0.35f, 1f);
            endPosition = Helper.Cam.GetRightSideRandomPos(_offsetFromBounds, 0f, 0f, 0.8f);
        }
        else // start from the right
        {
            startPosition = Helper.Cam.GetRightSideRandomPos(_offsetFromBounds, 0f, 0.35f, 1f);
            endPosition = Helper.Cam.GetLeftSideRandomPos(_offsetFromBounds, 0f, 0f, 0.8f);
        }
        Vector2 moveDir = (endPosition - startPosition).normalized;
        _rb.position = startPosition;
        transform.position = startPosition;
        _rb.velocity = moveDir * _floatingSpeed;
        //_estimatedFlyToTime = Vector2.Distance(_targetPosition, startPosition) / _floatingSpeed;
        //_floatingTime = 0f;
        _rb.angularVelocity = _rotateSpeed * Random.Range(-1f, 1f);
        _prePosition = startPosition;
        _health.SetHealth(_health.GetMaxHealth());
    }

    private void FixedUpdate()
    {
        //_floatingTime += Time.fixedDeltaTime;
        //var distance = Vector2.Distance(_rb.position, _targetPosition);
        //if (Mathf.Approximately(distance, 0) || (_floatingTime >= _estimatedFlyToTime))
        //{
        //    if (_resetOnOffScreen)
        //        Initialize();
        //    else
        //        Destroy(gameObject, 0.5f);
        //}

        bool IsOnScreenBefore = Helper.Cam.IsPositionInWorldCamRect(_prePosition, _offsetFromBounds);
        bool IsOffScreenNow = !Helper.Cam.IsPositionInWorldCamRect(_rb.position, _offsetFromBounds);
        if (IsOnScreenBefore && IsOffScreenNow)
        {
            Debug.Log("Asteroid is offscreen!");
            Deactivate();
        }
        _prePosition = _rb.position;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //if (collision.CompareTag(PlaySceneGlobal.Instance.Tag_Player))
        //{
        //    if (Time.time >= _hitNextTime)
        //    {
        //        Player player = collision.attachedRigidbody.GetComponent<Player>();
        //        player.TakeDamage(_damagePerHit, true);
        //        _hitNextTime = Time.time + _hitRate;
        //    }
        //}

        DamagableCollider hitCollider = collision.GetComponent<DamagableCollider>();
        if (hitCollider != null)
        {
            if (hitCollider.CompareTag(PlaySceneGlobal.Instance.Tag_PlayerBullet))
            {
                var bullet = hitCollider.GetComponent<BulletBase>();
                if (bullet != null)
                    bullet.TriggerHitVFX();
                bool isCritical = false;
                int damage = hitCollider.GetCalculatedDamage(out isCritical);
                TakeDamage(damage, isCritical);
                Destroy(hitCollider.gameObject);
            }
        }
    }

    public void TakeDamage(int damage, bool isCritical = false)
    {
        _health.SetHealth(_health.GetHealth() - Mathf.Max(0, damage));
        DamagePopup.Create(damage, transform.position, isCritical);
        if (_health.GetHealth() <= 0)
        {
            OnDied();
        }
    }
    private void OnDied()
    {
        if (_explosionEffect != null)
            Instantiate(_explosionEffect, transform.position, Quaternion.identity, PlaySceneGlobal.Instance.VFXParent);

        Deactivate();
    }

    private void Deactivate()
    {
        if (_pooledProduct != null)
        {
            _rb.velocity = Vector3.zero;
            _rb.angularVelocity = 0;
            transform.position = Vector3.zero;
            transform.rotation = Quaternion.identity;
            _pooledProduct.Release();
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
