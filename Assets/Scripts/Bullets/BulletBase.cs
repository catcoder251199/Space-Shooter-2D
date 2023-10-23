using UnityEngine;

public abstract class BulletBase : MonoBehaviour
{
    [SerializeField] protected DamagableCollider _damageCollider;
    [SerializeField] protected float _speed = 1f;
    public float Speed { set { _speed = value; } }

    public DamagableCollider DamagableCollider => _damageCollider;
    public abstract void TriggerHitVFX();
}