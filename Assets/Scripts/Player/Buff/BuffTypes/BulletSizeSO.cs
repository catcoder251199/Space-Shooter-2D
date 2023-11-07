using UnityEngine;

namespace Buff
{
    [CreateAssetMenu(fileName = "New Bullet Size", menuName = "ScriptableObjects/Buff/Bullet Size")]
    public class BulletSizeSO : BuffSO
    {
        public float scaleInc = 1;
        public int speedInc = 1;

        public override TimedBuff Initialize(GameObject gameObject)
        {
            return new BulletSizeBuff(this, gameObject);
        }
    }
}

