using UnityEngine;

namespace Buff
{
    [CreateAssetMenu(fileName = "New Split/Triple Pattern", menuName = "ScriptableObjects/Buff/Shoot Pattern/Split or Triple")]
    public class SplitPatternSO : ShootPatternSO
    {
        public override TimedBuff Initialize(GameObject gameObject)
        {
            return new SwapSplitPattern(this, gameObject);
        }
    }
}

