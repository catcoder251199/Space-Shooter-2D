using PlayerNS;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    public UnityEvent OnPlayerDiedEvent;

    [SerializeField] private PlayerController _controller;
    [SerializeField, Header("Basic stats")] private int _maxHp = 100;
    [SerializeField] private float _fireRate = 0.5f;

    private Health _health;
    private PlayerDamageHandler _damageHandler;
    private PlayerBulletPool _bulletPool;
    private WeaponHandler _weaponHandler;

    public int Damage => 0;
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

    private void Awake()
    {
        _health = GetComponent<Health>();
        MaxHp = _maxHp;

        _damageHandler = GetComponent<PlayerDamageHandler>();
        _bulletPool = GetComponent<PlayerBulletPool>();

        _weaponHandler = GetComponent<WeaponHandler>();
        _weaponHandler.ChangeShootPattern(new GatlingPattern(_weaponHandler));
    }

    private void Start()
    {
        _weaponHandler?.Activate();
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
        Debug.Log("Player destroyed -> Game Over !");
        _weaponHandler.Deactivate();
        OnPlayerDiedEvent?.Invoke();
    }
}
