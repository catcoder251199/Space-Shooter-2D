using PlayerNS;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    public UnityEvent OnPlayerDiedEvent;

    [SerializeField, Header("Custom look")] private SpriteRenderer _airshipSpriteRenderer;
    [SerializeField] private ParticleSystem _trailEffect;
    [SerializeField, Header("Basic stats")] private int _initialHealth = 100;
    [SerializeField] private float _fireRate = 0.5f;
    [SerializeField, Header("Effects")] private ParticleSystem _explosionEffect;
    [SerializeField] private ParticleSystem _poweredUpEffect;
    [SerializeField] private ParticleSystem _depoweredUpEffect;
    [SerializeField] private ParticleSystem _healEffect;
    [SerializeField] private GameObject _shield;
    [SerializeField, Header("Sound")] private AudioClip _explosionSound;
    [SerializeField] private AudioClip _poweredUpSound;

    private Health _health;
    private PlayerDamageHandler _damageHandler;
    private PlayerBulletPool _bulletPool;
    private WeaponHandler _weaponHandler;
    private PlayerController _controller;

    public PlayerController Controller => _controller;
    public WeaponHandler WeaponHandler => _weaponHandler;

    public Health Health => _health;
    public float FireRate { get => _fireRate; set => _fireRate = value; } // time between 2 sequential bullets
    public PlayerDamageHandler DamageHandler => _damageHandler;
    public PlayerBulletPool BulletPool => _bulletPool;
    public IShootPattern DefaultShootPattern => new SingleShotPattern(_weaponHandler);

    private void Awake()
    {
        _health = GetComponent<Health>();

        _damageHandler = GetComponent<PlayerDamageHandler>();
        _bulletPool = GetComponent<PlayerBulletPool>();
        _controller = GetComponent<PlayerController>();
        _weaponHandler = GetComponent<WeaponHandler>();
        _weaponHandler.ChangeShootPattern(DefaultShootPattern);
    }

    public void Init(SpaceShipSO spaceshipInfo)
    {
        _airshipSpriteRenderer.sprite = spaceshipInfo.spaceShipSprite;
        _trailEffect.startColor = spaceshipInfo.trailColor;
        _bulletPool.SetNewBulletPrefab(spaceshipInfo.bulletPrefab);

        // base stats
        _initialHealth = spaceshipInfo.GetCurrentHp();
        _health.SetMaxHealth(_initialHealth);
        _health.SetHealth(_initialHealth);

        _damageHandler.SetDamage(spaceshipInfo.GetCurrentBaseDamage());
        _damageHandler.SetCritModifier(spaceshipInfo.baseCritDamageModifier);
        _damageHandler.SetCritRate(spaceshipInfo.baseCritRate);

        _controller.SetSpeed(spaceshipInfo.baseSpeed);
    }

    public void Init()
    {
        _health.SetMaxHealth(_initialHealth);
        _health.SetHealth(_initialHealth);
    }

    private void Start()
    {
        var spawnManager = GameManager.Instance?.SpawnManager;
        if (spawnManager != null )
            spawnManager.onDamageableSpawnableCountChanged += onDamageableEnemyCountChanged;
    }

    private void OnDisable()
    {
        var spawnManager = GameManager.Instance?.SpawnManager;
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
        Debug.Log("Player destroyed !");
        _weaponHandler.Deactivate();
        _controller.enabled = false;

        if (_explosionEffect != null)
            Instantiate(_explosionEffect, transform.position, Quaternion.identity, PlaySceneGlobal.Instance.VFXParent);

        if (_explosionSound != null)
            SoundManager.Instance.PlayEffectOneShot(_explosionSound);

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
        var currentPattern = _weaponHandler.CurrentPattern;
        if (pattern != null && (pattern.GetType() != currentPattern.GetType()))
        {
            _weaponHandler.ChangeShootPattern(pattern);
            _weaponHandler.Activate();
        }
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

    public void SetShieldEnabled(bool enabled)
    {
        _shield.SetActive(enabled);
        _health.SetDamageable(!enabled);
    }

    public void TriggerPoweredUpEffect()
    {
        _poweredUpEffect.Play();
        if (_poweredUpSound != null)
            SoundManager.Instance.PlayEffectOneShot(_poweredUpSound);
    }

    public void TriggerDePoweredUpEffect()
    {
        _depoweredUpEffect.Play();
    }

    public void TriggerHealEffect()
    {
        _healEffect.Play();
        if (_poweredUpSound != null)
            SoundManager.Instance.PlayEffectOneShot(_poweredUpSound);
    }
}
