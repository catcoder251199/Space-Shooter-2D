using UnityEngine;

public abstract class BulletBase : MonoBehaviour
{
    [SerializeField] protected DamagableCollider _damageColider;
    [SerializeField] protected float _speed = 1f;
    public float Speed { set { _speed = value; } }

    public DamagableCollider DamagableCollider => _damageColider;
    public abstract void TriggerHitVFX();
}