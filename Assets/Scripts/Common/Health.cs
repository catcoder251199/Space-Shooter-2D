using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public IntEvent OnHealthChanged;
    public IntEvent OnMaxHealthChanged;
    public IntIntEvent OnNewHealthSet;
    public IntIntEvent OnNewMaxHealthSet;

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

        OnHealthChanged?.Invoke(newHealth);
        OnNewHealthSet?.Invoke(_currentHealth, newHealth);

        _currentHealth = newHealth;
    }

    public void SetMaxHealth(int newMaxHealth)
    {
        newMaxHealth = Mathf.Max(newMaxHealth, 0);
        //if (newMaxHealth == _maxHealth)
        //    return;
        
        OnMaxHealthChanged?.Invoke(newMaxHealth);
        OnNewMaxHealthSet?.Invoke(_maxHealth, newMaxHealth);

        _maxHealth = newMaxHealth;

        if (_maxHealth < _currentHealth)
        {
            SetHealth(_maxHealth);
        }
    }

    public int GetHealth() => _currentHealth;
    public int GetMaxHealth() => _maxHealth;

}
