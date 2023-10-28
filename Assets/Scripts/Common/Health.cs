using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public IntIntEvent OnNewHealthSet; // (int oldHealth, int newHealth)
    public IntIntEvent OnNewMaxHealthSet; // (int oldMaxHealth, int newMaxHealth)
    public IntBoolEvent OnHealthDamaged; // (int damage, bool isCritical)

    [SerializeField] int _maxHealth;
    [SerializeField] int _currentHealth;

    private void Awake()
    {
        SetMaxHealth(_maxHealth);
        SetHealth(_maxHealth);
    }

    public void SetHealth(int newHealth)
    {
        newHealth = Mathf.Clamp(newHealth, 0, _maxHealth);

        //if (newHealth == _currentHealth)
        //    return;

        OnNewHealthSet?.Invoke(_currentHealth, newHealth);

        _currentHealth = newHealth;
    }

    public void SetMaxHealth(int newMaxHealth)
    {
        newMaxHealth = Mathf.Max(newMaxHealth, 0);
        //if (newMaxHealth == _maxHealth)
        //    return;
        
        OnNewMaxHealthSet?.Invoke(_maxHealth, newMaxHealth);

        _maxHealth = newMaxHealth;

        if (_maxHealth < _currentHealth)
        {
            SetHealth(_maxHealth);
        }
    }

    public int GetHealth() => _currentHealth;
    public int GetMaxHealth() => _maxHealth;

    public void TakeDamage(int damage, bool isCritical = false, bool notifyZeroDamage = false)
    {
        damage = Mathf.Max(0, damage); // passed damage argument must be positive
        if (damage > 0 || notifyZeroDamage)
        {
            SetHealth(GetHealth() - damage);
            OnHealthDamaged?.Invoke(damage, isCritical);
        }
    }
}
