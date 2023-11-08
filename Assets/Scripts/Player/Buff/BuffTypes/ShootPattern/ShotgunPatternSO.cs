using UnityEngine;

namespace Buff
{
    [CreateAssetMenu(fileName = "New Shotgun Pattern", menuName = "ScriptableObjects/Buff/Shoot Pattern/Shotgun")]
    public class ShotgunPatternSO : ShootPatternSO
    {
        public override TimedBuff Initialize(GameObject gameObject)
        {
            return new SwapShotgunPattern(this, gameObject);
        }
    }
}

