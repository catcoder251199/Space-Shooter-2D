using PlayerNS;
using UnityEngine;


namespace Buff
{
    public class SwapShotgunPattern : SwapShootPattern
    {
        public SwapShotgunPattern(ShootPatternSO buff, GameObject gameObject) : base(buff, gameObject) { }
        public override IShootPattern CreateShootPattern() => new ShotgunPattern(_player.WeaponHandler);
    }
}