using Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PlayerBullet : BulletBase
{
    public enum DestroyedCondition
    {
        LifeTime,
        OutOfScreen
    }
    [SerializeField] private DestroyedCondition _destroyedCondition;
    [SerializeField] private float _lifeTime = 3f;
    [SerializeField] private float _screenOffset = 3f;
    private float _existedTime = 0f;

    [SerializeField] private ParticleSystem _hitVFX;
    
    private Rigidbody2D _rb;
    private OneHitCollider _hitCollider;

    [SerializeField] private float _speed = 1f;
    [SerializeField] private int _damage = 1;
    [SerializeField] private bool _isCritical = false;

    public IObjectPool<PlayerBullet> bulletPool;

    public float Speed { set => _speed = value; get => _speed; }
    public override int Damage 
    { 
        get => _damage; 
        set 
        {
            _hitCollider.damage = value;
            _damage = value;
        }
    }
    public override bool IsCritical 
    { 
        get => _isCritical;
        set
        {
            _hitCollider.isCritical = value;
            _isCritical = value;
        }
    }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _hitCollider = GetComponent<OneHitCollider>();
    }
    public void Initialize()
    {
        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = 0f;
        _existedTime = 0;
    }

    private void Update()
    {
        if (_destroyedCondition == DestroyedCondition.LifeTime)
        {
            if (_existedTime < _lifeTime)
                _existedTime += Time.deltaTime;

            if (_existedTime >= _lifeTime)
                Deactivate();
        }
        else if (_destroyedCondition == DestroyedCondition.OutOfScreen)
        {
            if (!IsInScreen())
                Deactivate();
        }
    }

    public bool IsInScreen()
    {
        return Helper.Cam.IsPositionInWorldCamRect(transform.position, _screenOffset);
    }

    private void FixedUpdate()
    {
        _rb.velocity = transform.up * _speed;
    }

    public void TriggerHitVFX()
    {
        if (_hitVFX != null ) 
            Instantiate(_hitVFX, transform.position, Quaternion.identity, PlaySceneGlobal.Instance.VFXParent);
    }

    public void Deactivate()
    {
        if (bulletPool != null)
        {
            transform.localScale = Vector2.one;
            bulletPool.Release(this);
        }
        else
            Destroy(gameObject);
    }
    public void OnTriggerEnteredEventHappened(Collider2D collision)
    {
        TriggerHitVFX();
        // If the bullet hit something, then deactivate it
        Deactivate();
    }
}
