using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageHandler : MonoBehaviour
{
    [SerializeField] int _damage;
    [SerializeField, Range(0, 3)] float _critDmgModifier = 0.1f;
    [SerializeField, Range(0, 1)] float _critRate = 0.1f;
    public void SetCritRate(float critRate) => _critRate = Mathf.Max(critRate, 0f);
    public void SetCritModifier(float critModifier) => _critDmgModifier = critModifier;
    public float GetCritRate() => _critRate;
    public bool IsCritMaximized() => GetCritRate() >= 1f;
    public int GetDamage() => _damage;
    public int GetCritDamage() => _damage + Mathf.RoundToInt(_damage * _critDmgModifier);
    public int GetCalculatedDamage(out bool isCritical)
    {
       isCritical = Random.Range(0f, 1f) < GetCritRate();
       return isCritical ? GetCritDamage() : GetDamage();
    }
    public void SetDamage(int damage) => _damage = Mathf.Max(0, damage);
}
