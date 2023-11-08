using UnityEngine;

namespace Buff
{
    [CreateAssetMenu(fileName = "New Max Hp Up", menuName = "ScriptableObjects/Buff/Max Hp Up")]
    public class MaxHpUpSO : BuffSO
    {
        public int incAmount = 0;

        public override TimedBuff Initialize(GameObject gameObject)
        {
            return new MaxHpUpBuff(this, gameObject);
        }
    }
}

