using UnityEngine;

namespace Buff
{
    [CreateAssetMenu(fileName = "New Firing Rate", menuName = "ScriptableObjects/Buff/Firing Rate")]
    public class FiringRateSO : BuffSO
    {
        public int firingStackInc = 1;

        public override TimedBuff Initialize(GameObject gameObject)
        {
            return new FiringRateBuff(this, gameObject);
        }
    }
}

