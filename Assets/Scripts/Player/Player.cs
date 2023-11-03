using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    public UnityEvent OnPlayerDiedEvent;
    public enum AttackBehaviourType
    {
        Single = 0,
        Double,
        Splitter
    };

    [SerializeField] private PlayerController _controller;
    [SerializeField] private List<AttackBehaviourSO> _attackBehaviourData;
    [SerializeField] private AttackBehaviour _attackBehaviour;
    [SerializeField] private int _maxHp = 100;

    private Health _health;
    private PlayerDamageHandler _damageHandler;
    private PlayerBulletPool _bulletPool;

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
    public PlayerDamageHandler DamageHandler => _damageHandler;
    public PlayerBulletPool BulletPool => _bulletPool;

    private void Awake()
    {
        _health = GetComponent<Health>();
        MaxHp = _maxHp;

        _damageHandler = GetComponent<PlayerDamageHandler>();
        _bulletPool = GetComponent<PlayerBulletPool>();
    }

    private void Start()
    {
        _attackBehaviour.StartDoing();
    }
    public AttackBehaviourSO GetAttackBehaviourData(AttackBehaviourType type) => _attackBehaviourData[(int)type];

    public bool IsAlive() => Health.GetHealth() > 0;

    public void TakeDamage(int damage, bool isCritical = false)
    {
        _health.TakeDamage(damage);
    }

    public void OnDamageTaken(int damage, bool isCritical = false)
    {
        DamagePopup.Create(damage, transform.position, isCritical);
        if (_health.GetHealth() <= 0)
        {
            OnPlayerDied();
        }
    }

    private void OnPlayerDied()
    {
        Debug.Log("Player destroyed -> Game Over !");
        _attackBehaviour.StopDoing();
        OnPlayerDiedEvent?.Invoke();
    }
}
