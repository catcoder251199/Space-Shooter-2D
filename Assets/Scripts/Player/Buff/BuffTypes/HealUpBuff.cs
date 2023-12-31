using UnityEngine;

namespace Buff
{
    public class HealUpBuff : TimedBuff
    {
        private Player _player;
        public HealUpBuff(BuffSO buff, GameObject obj) : base(buff, obj)
        {
            _player = obj.GetComponent<Player>();
        }

        protected override void OnActivated()
        {
            _player.TriggerHealEffect();
            //var uiManager = GameManager.Instance?.UIManager;
            //if (uiManager != null)
            //    uiManager.ShowBuffDescriptionPanel(Buff);
        }

        protected override void ApplyEffect()
        {
            HealUpSO buff = Buff as HealUpSO;
            _player.SetHealUpWith(buff.incAmount);
        }

        public override void OnFinished()
        {
        }
    }
}

