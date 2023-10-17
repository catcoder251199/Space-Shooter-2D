using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagableCollider : MonoBehaviour
{
   [SerializeField] int _damage;
    public int GetDamage() => _damage;
    public void SetDamage(int damage)
    {
        _damage = Mathf.Max(0, damage);
    }
}
