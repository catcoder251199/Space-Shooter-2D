using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class OneHitCollider : DamageColliderBase 
    {
        public Collider2D_Event OnTriggerEnteredEvent;
        protected override void  Awake()
        {
            base.Awake();
        }

        protected void OnTriggerEnter2D(Collider2D collision)
        {
            if (CompareDamageableTag(collision.attachedRigidbody.tag))
            {
                Health health = collision.attachedRigidbody.GetComponent<Health>();
                health.TakeDamage(damage, isCritical, true);
            }

            OnTriggerEnteredEvent?.Invoke(collision);
        }
    }
}