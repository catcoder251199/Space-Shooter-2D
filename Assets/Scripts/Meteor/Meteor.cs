using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Meteor : MonoBehaviour
{
    [SerializeField] bool _resetOnOffScreen = false;
    [SerializeField] float _floatingSpeedMin = 1f;
    [SerializeField] float _floatingSpeedMax = 1.5f;
    float _floatingSpeed = 1f;
    [SerializeField] float _minScaleXY = 1f;
    [SerializeField] float _maxScaleXY = 1.5f;

    [SerializeField] float _minRotateSpeed = 30;
    [SerializeField] float _maxRotateSpeed = 60;
    float _rotateSpeed = 360f;

    [SerializeField] float _offsetFromBounds = 2f; // in pixels

    [SerializeField] GameObject[] _meteorVisuals;
    GameObject _visual;
    [SerializeField] ParticleSystem _explosionEffect;

    [SerializeField, Header("Hit")] float _hitRate = 1f;
    private float _hitNextTime = -1f;
    [SerializeField] private int _damagePerHit = 10;

    private Vector2 _moveDir;
    private Rigidbody2D _rb2D;
    private Vector2 _targetPosition;
    private float _estimatedFlyToTime = -1f;
    private float _floatingTime = 0f;
    private Health _health;
    private void Awake()
    {
        _rb2D = GetComponent<Rigidbody2D>();
        _health = GetComponent<Health>();
    }

    private void Start()
    {
        float randomScale = Random.Range(_minScaleXY, _maxScaleXY);
        transform.localScale = new Vector3(randomScale, randomScale, 1f);
        Initialize();
    }

    private void Initialize()
    {
        if (_visual == null)
            _visual = Instantiate(_meteorVisuals[Random.Range(0, _meteorVisuals.Length - 1)], Vector3.zero, Quaternion.identity, this.transform);


        _floatingSpeed = Random.Range(_floatingSpeedMin, _floatingSpeedMax);
        _rotateSpeed = Random.Range(_minRotateSpeed, _maxRotateSpeed);

        bool startFromLeft = Random.value > 0.5f;
        Vector2 startPosition;
        if (startFromLeft)
        {
            startPosition = Helper.Cam.GetLeftSideRandomPos(_offsetFromBounds, 0f, 0.35f, 1f);
            _targetPosition = Helper.Cam.GetRightSideRandomPos(_offsetFromBounds, 0f, 0f, 0.8f);
        }
        else // start from the right
        {
            startPosition = Helper.Cam.GetRightSideRandomPos(_offsetFromBounds, 0f, 0.35f, 1f);
            _targetPosition = Helper.Cam.GetLeftSideRandomPos(_offsetFromBounds, 0f, 0f, 0.8f);
        }
        _moveDir = (_targetPosition - startPosition).normalized;
        this.transform.position = startPosition;
        _rb2D.velocity = _moveDir * _floatingSpeed;
        _estimatedFlyToTime = Vector2.Distance(_targetPosition, startPosition) / _floatingSpeed;
        _floatingTime = 0f;
        _rb2D.angularVelocity = _rotateSpeed * Random.Range(-1f, 1f);
    }

    private void FixedUpdate()
    {
        _floatingTime += Time.fixedDeltaTime;
        var distance = Vector2.Distance(_rb2D.position, _targetPosition);
        if (Mathf.Approximately(distance, 0) || (_floatingTime >= _estimatedFlyToTime))
        {
            if (_resetOnOffScreen)
                Initialize();
            else
                Destroy(gameObject, 0.5f);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag(PlaySceneGlobal.Instance.Tag_Player))
        {
            if (Time.time >= _hitNextTime)
            {
                Player player = collision.attachedRigidbody.GetComponent<Player>();
                player.TakeDamage(_damagePerHit, true);
                _hitNextTime = Time.time + _hitRate;
            }
        }

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

    private void TakeDamage(int damage, bool isCritical = false)
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

        Destroy(gameObject, 0.1f);
    }
}
