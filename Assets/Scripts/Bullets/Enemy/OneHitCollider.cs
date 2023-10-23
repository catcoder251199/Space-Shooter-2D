using UnityEngine;

namespace Enemy
{
    public class OneHitCollider : EnemyHitColliderBase 
    {
        public bool destroyOnHit = false;
        public bool IsCritical = false;

        protected void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(PlaySceneGlobal.Instance.Tag_Player))
            {
                Player player = collision.attachedRigidbody.GetComponent<Player>();
                player.TakeDamage(damage, IsCritical);
                if (destroyOnHit)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}