using UnityEngine;

namespace Buff
{
    [CreateAssetMenu(fileName = "New Shield Up", menuName = "ScriptableObjects/Buff/Shield")]
    public class ShieldUpSO : BuffSO
    {
        public override TimedBuff Initialize(GameObject gameObject)
        {
            return new ShieldUpBuff(this, gameObject);
        }
    }
}

