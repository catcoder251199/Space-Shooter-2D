using PlayerNS;
using UnityEngine;


namespace Buff
{
    public class SwapDoublePattern : SwapShootPattern
    {
        public SwapDoublePattern(ShootPatternSO buff, GameObject gameObject) : base(buff, gameObject) { }
        public override IShootPattern CreateShootPattern() => new DoubleShotPattern(_player.WeaponHandler);
    }
}