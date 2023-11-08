using UnityEngine;

namespace Buff
{
    [CreateAssetMenu(fileName = "New Double Pattern", menuName = "ScriptableObjects/Buff/Shoot Pattern/Double")]
    public class DoublePatternSO : ShootPatternSO
    {
        public override TimedBuff Initialize(GameObject gameObject)
        {
            return new SwapDoublePattern(this, gameObject);
        }
    }
}

