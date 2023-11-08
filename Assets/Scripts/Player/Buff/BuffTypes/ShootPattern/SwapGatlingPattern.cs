using PlayerNS;
using UnityEngine;


namespace Buff
{
    public class SwapGatlingPattern : SwapShootPattern
    {
        public SwapGatlingPattern(ShootPatternSO buff, GameObject gameObject) : base(buff, gameObject) { }
        public override IShootPattern CreateShootPattern() => new GatlingPattern(_player.WeaponHandler);
    }
}