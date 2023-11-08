using UnityEngine;

namespace Buff
{
    [CreateAssetMenu(fileName = "New Gatling Pattern", menuName = "ScriptableObjects/Buff/Shoot Pattern/Gatling")]
    public class GatlingPatternSO : ShootPatternSO
    {
        public override TimedBuff Initialize(GameObject gameObject)
        {
            return new SwapGatlingPattern(this, gameObject);
        }
    }
}

