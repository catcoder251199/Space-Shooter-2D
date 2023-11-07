using UnityEngine;

namespace Buff
{
    [CreateAssetMenu(fileName = "New Crit Damage", menuName = "ScriptableObjects/Buff/Crit Damage")]
    public class CritDamageSO : BuffSO
    {
        public float critModifier = 0;

        public override TimedBuff Initialize(GameObject gameObject)
        {
            return new CritDamageBuff(this, gameObject);
        }
    }
}

