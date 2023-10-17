using UnityEngine;

public abstract class BulletBase : MonoBehaviour
{
    [SerializeField] DamagableCollider _damageColider;
    public DamagableCollider DamagableCollider => _damageColider;
    public abstract void TriggerHitVFX();
}