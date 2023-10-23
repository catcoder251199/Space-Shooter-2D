using UnityEngine;

namespace Enemy
{
    public class ConstantHitCollider : EnemyHitColliderBase
    {
        public float hitRate = 1f;
        public bool isCritical = true;
        private float _hitNextTime = 0f;

        protected void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.CompareTag(PlaySceneGlobal.Instance.Tag_Player))
            {
                if (Time.time >= _hitNextTime)
                {
                    _hitNextTime = Time.time + hitRate;
                    Player player = collision.attachedRigidbody.GetComponent<Player>();
                    player.TakeDamage(damage, isCritical);
                }
            }
        }
    }
}