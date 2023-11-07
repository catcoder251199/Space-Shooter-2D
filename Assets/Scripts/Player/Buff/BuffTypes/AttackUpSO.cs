using UnityEngine;

namespace Buff
{
    [CreateAssetMenu(fileName = "New Attack Up", menuName = "ScriptableObjects/Buff/Attack Up")]
    public class AttackUpSO : BuffSO
    {
        public int attackIncrease = 0;

        public override TimedBuff Initialize(GameObject gameObject)
        {
            return new AttackUpBuff(this, gameObject);
        }
    }
}

