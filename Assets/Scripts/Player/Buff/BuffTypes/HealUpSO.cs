using UnityEngine;

namespace Buff
{
    [CreateAssetMenu(fileName = "New Heal Up", menuName = "ScriptableObjects/Buff/Heal Up")]
    public class HealUpSO : BuffSO
    {
        public int incAmount = 0;

        public override TimedBuff Initialize(GameObject gameObject)
        {
            return new HealUpBuff(this, gameObject);
        }
    }
}

