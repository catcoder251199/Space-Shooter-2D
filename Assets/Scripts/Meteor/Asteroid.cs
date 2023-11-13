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

    // Object Pool
    [SerializeField, Header("Pooled Spawnable Object/Product")] private PooledSpawnableProduct _pooledProduct;
    [SerializeField] private AudioClip _explosionSound;
    
    private Rigidbody2D _rb;
    private Health _health;

    [SerializeField]  private float _delayDestroyTime = 1.5f; // after the asteroid is off screen, we count down to zero to destroy it
    private float _countDownDestroyTime = 0f;
    private bool _isOnScreenOnce = false;
    private bool _spawnLeft = false;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _health = GetComponent<Health>();
    }

    private void Start()
    {
        if (_playOnStart)
            Initialize();
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
            startPosition = Helper.Cam.GetLeftSideRandomPos(_offsetFromBounds, 0f, 0.2f, 1f);
            endPosition = Helper.Cam.GetRightSideRandomPos(_offsetFromBounds, 0f, 0f, 0.8f);
            _spawnLeft = true;
        }
        else // start from the right
        {
            startPosition = Helper.Cam.GetRightSideRandomPos(_offsetFromBounds, 0f, 0.2f, 1f);
            endPosition = Helper.Cam.GetLeftSideRandomPos(_offsetFromBounds, 0f, 0f, 0.8f);
            _spawnLeft = false;
        }
        Vector2 moveDir = (endPosition - startPosition).normalized;
        _rb.position = startPosition;
        transform.position = startPosition;
        _rb.velocity = moveDir * _floatingSpeed;
        _rb.angularVelocity = _rotateSpeed * Random.Range(-1f, 1f);
        _health.SetHealth(_health.GetMaxHealth());

        _isOnScreenOnce = false;
        _countDownDestroyTime = _delayDestroyTime;
    }

    private void Update()
    {
        if (GameManager.gameIsPaused)
            return;

        bool IsOnScreenNow = Helper.Cam.IsPositionInWorldCamRect(transform.position, _offsetFromBounds);
        if (!_isOnScreenOnce && IsOnScreenNow)
            _isOnScreenOnce = true;

        //if (IsOnScreenNow)
        //    _countDownDestroyTime = _delayDestroyTime; // whenever it moves to screen, reset the count down time
        //else if (_isOnScreenOnce) // if it leaves the screen and has been on screen once, then count down time to zero to destroy it
        //    _countDownDestroyTime -= Time.deltaTime;

        if (IsOnScreenNow)
            _countDownDestroyTime = _delayDestroyTime; // whenever it moves to screen, reset the count down time
        else if (_isOnScreenOnce) // if it leaves the screen and has been on screen once, then count down time to zero to destroy it
        {
            if (_spawnLeft && transform.position.x >= Helper.Cam.WorldRight() + _offsetFromBounds)
                _countDownDestroyTime -= Time.deltaTime;
            if (!_spawnLeft && transform.position.x <= Helper.Cam.WorldLeft() - _offsetFromBounds)
                _countDownDestroyTime -= Time.deltaTime;
        }

        if (_countDownDestroyTime <= 0)
            Deactivate();
    }

    private void FixedUpdate()
    {
    }

    public void OnTakeDamage(int damage, bool isCritical = false)
    {
        DamagePopup.Create(damage, transform.position, isCritical);
        if (_health.GetHealth() <= 0)
            OnDied();
    }

    private void OnDied()
    {
        if (_explosionEffect != null)
            Instantiate(_explosionEffect, transform.position, Quaternion.identity, PlaySceneGlobal.Instance.VFXParent);

        if (_explosionSound != null)
            SoundManager.Instance.PlayEffectOneShot(_explosionSound);
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
            Destroy(gameObject);
    }
}
