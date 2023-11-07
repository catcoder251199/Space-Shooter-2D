using UnityEngine;

namespace Buff
{
    [CreateAssetMenu(fileName = "New Crit Rate", menuName = "ScriptableObjects/Buff/Crit Rate")]
    public class CritRateSO : BuffSO
    {
        public float critRateIncrease = 0;

        public override TimedBuff Initialize(GameObject gameObject)
        {
            return new CritRateBuff(this, gameObject);
        }
    }
}

