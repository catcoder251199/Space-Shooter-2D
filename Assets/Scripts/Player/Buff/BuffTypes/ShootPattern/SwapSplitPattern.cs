using PlayerNS;
using UnityEngine;


namespace Buff
{
    public class SwapSplitPattern : SwapShootPattern
    {
        public SwapSplitPattern(ShootPatternSO buff, GameObject gameObject) : base(buff, gameObject) { }
        public override IShootPattern CreateShootPattern() => new SplitShotPattern(_player.WeaponHandler);
    }
}