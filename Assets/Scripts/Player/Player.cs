using PlayerNS;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.XR;

public class Player : MonoBehaviour
{
    public UnityEvent OnPlayerDiedEvent;

    [SerializeField, Header("Basic stats")] private int _maxHp = 100;
    [SerializeField] private float _fireRate = 0.5f;
    [SerializeField] private ParticleSystem _explosionEffect;
    [SerializeField] private ParticleSystem _poweredUpEffect;
    [SerializeField] private ParticleSystem _depoweredUpEffect;

    private Health _health;
    private PlayerDamageHandler _damageHandler;
    private PlayerBulletPool _bulletPool;
    private WeaponHandler _weaponHandler;
    private PlayerController _controller;

    public PlayerController Controller => _controller;
    public WeaponHandler WeaponHandler => _weaponHandler;

    public int MaxHp 
    {
        get { return _maxHp; }
        set
        {
            _maxHp = value;
            _health.SetMaxHealth(_maxHp);
        }
    }
    public Health Health => _health;
    public float FireRate { get => _fireRate; set => _fireRate = value; } // time between 2 sequential bullets
    public PlayerDamageHandler DamageHandler => _damageHandler;
    public PlayerBulletPool BulletPool => _bulletPool;
    public IShootPattern DefaultShootPattern => new SingleShotPattern(_weaponHandler);

    private void Awake()
    {
        _health = GetComponent<Health>();
        MaxHp = _maxHp;

        _damageHandler = GetComponent<PlayerDamageHandler>();
        _bulletPool = GetComponent<PlayerBulletPool>();
        _controller = GetComponent<PlayerController>();
        _weaponHandler = GetComponent<WeaponHandler>();
        _weaponHandler.ChangeShootPattern(DefaultShootPattern);
    }

    private void Start()
    {
        var spawnManager = GameManager.Instance.SpawnManager;
        if (spawnManager != null )
            spawnManager.onDamageableSpawnableCountChanged += onDamageableEnemyCountChanged;
    }

    private void OnDisable()
    {
        var spawnManager = GameManager.Instance.SpawnManager;
        if (spawnManager != null)
            spawnManager.onDamageableSpawnableCountChanged -= onDamageableEnemyCountChanged;
    }

    public bool IsAlive() => Health.GetHealth() > 0;

    public void TakeDamage(int damage, bool isCritical = false)
    {
        _health.TakeDamage(damage);
    }

    public void OnDamageTaken(int damage, bool isCritical = false)
    {
        DamagePopup.Create(damage, transform.position, isCritical);
        if (_health.GetHealth() <= 0)
            OnPlayerDied();
    }

    private void OnPlayerDied()
    {
        Debug.Log("Player destroyed !!!");
        _weaponHandler.Deactivate();
        _controller.enabled = false;
        Instantiate(_explosionEffect, transform.position, Quaternion.identity, PlaySceneGlobal.Instance.VFXParent);
        OnPlayerDiedEvent?.Invoke();
        gameObject.SetActive(false);
    }

    private void onDamageableEnemyCountChanged(int prevCount, int newCount)
    {
        if (prevCount <= 0 && newCount > 0)
            _weaponHandler.Activate();
        else if (prevCount > 0 && newCount <= 0)
            _weaponHandler.Deactivate();
    }

    // ---Power ups---
    public void SetAttackUpWith(int amount)
    {
        _damageHandler.SetDamage(_damageHandler.GetDamage() + amount);
    }

    public void SetCritRateWith(float amount)
    {
        _damageHandler.SetCritRate(_damageHandler.GetCritRate() + amount);
    }

    public void SetCritDamageModifierWith(float amount)
    {
        _damageHandler.SetCritModifier(_damageHandler.GetCritModifier() + amount);
    }

    public void SetFiringRateStackWith(int amountStack)
    {
        _weaponHandler.FireRateStack += amountStack;
    }

    public void SetBonusBulletScaleWith(float amount)
    {
        _weaponHandler.BonusScale += amount;
    }

    public void SetBulletSpeedWith(int amount)
    {
        _weaponHandler.SpeedStack += amount;
    }

    public void ChangeShootPattern(IShootPattern pattern)
    {
        _weaponHandler.ChangeShootPattern(pattern);
        _weaponHandler.Activate();
    }

    public void SetHpMaxWith(int amount)
    {
        _health.SetMaxHealth(_health.GetMaxHealth() + amount);
        if (amount > 0)
            _health.SetHealth(_health.GetHealth() + amount);
    }

    public void SetHealUpWith(int amount)
    {
        _health.SetHealth(_health.GetHealth() + amount);
    }

    public void TriggerPoweredUpEffect()
    {
        //if (_poweredUpEffect != null && !_poweredUpEffect.isPlaying)
            _poweredUpEffect.Play();
    }

    public void TriggerDePoweredUpEffect()
    {
        //if (_depoweredUpEffect != null && !_depoweredUpEffect.isPlaying)
            _depoweredUpEffect.Play();
    }
}
