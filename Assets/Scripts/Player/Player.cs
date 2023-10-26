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
    [SerializeField] private Health _health;

    [SerializeField, Header("Player Base Stats")] private int _damage = 100;
    [SerializeField ,Range(0, 1)] private float _critRate = 0.1f;
    [SerializeField, Range(0, 1)] private float _critModifier = 0.5f;
    [SerializeField] private int _maxHp = 100;
    public int Damge => _damage;
    public int MaxHp 
    {
        get { return _maxHp; }
        set
        {
            _maxHp = value;
            _health.SetMaxHealth(_maxHp);
        }
    }

    public float CritRate => Mathf.Clamp01(_critRate);
    public float CritModifier => Mathf.Max(_critModifier, 0);
    public Health Health => _health;

    private void Awake()
    {
        MaxHp = _maxHp;
    }

    public void SetCritRate(float critRate) =>_critRate = critRate;

    private void Start()
    {
        _attackBehaviour.StartDoing();
    }
    public AttackBehaviourSO GetAttackBehaviourData(AttackBehaviourType type) => _attackBehaviourData[(int)type];

    public bool IsAlive() => Health.GetHealth() > 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //DamagableCollider hitCollider = collision.GetComponent<DamagableCollider>();
        //if (hitCollider != null)
        //{
        //    if (hitCollider.CompareTag(PlaySceneGlobal.Instance.Tag_EnemyBullet))
        //    {
        //        TakeDamage(hitCollider.GetDamage());
        //        Destroy(hitCollider.gameObject);
        //    }
        //}
    }
    public void TakeDamage(int damage, bool isCritical = false)
    {
        _health.SetHealth(_health.GetHealth() - Mathf.Max(0, damage));
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
