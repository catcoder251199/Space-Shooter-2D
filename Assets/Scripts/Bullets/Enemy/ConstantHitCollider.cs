using UnityEngine;

namespace Enemy
{
    public class ConstantHitCollider : DamageColliderBase
    {
        public Collider2D_Event OnTriggerStayEvent;

        public float hitRate = 1f;
        private float _hitNextTime = 0f;

        protected void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.attachedRigidbody != null && CompareDamageableTag(collision.attachedRigidbody.tag))
            {
                if (Time.time >= _hitNextTime)
                {
                    _hitNextTime = Time.time + hitRate;
                    Health health = collision.attachedRigidbody.GetComponent<Health>();
                    health?.TakeDamage(damage, isCritical, true);
                }
            }

            OnTriggerStayEvent?.Invoke(collision);
        }
    }
}