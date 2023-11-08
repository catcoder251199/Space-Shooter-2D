using UnityEngine;

namespace Buff
{
    public class ShieldUpBuff : TimedBuff
    {
        private Player _player;
        public ShieldUpBuff(BuffSO buff, GameObject obj) : base(buff, obj)
        {
            _player = obj.GetComponent<Player>();
        }

        protected override void OnActivated()
        {
            _player.TriggerPoweredUpEffect();
            var uiManager = GameManager.Instance?.UIManager;
            if (uiManager != null)
                uiManager.ShowBuffDescriptionPanel(Buff);
        }

        protected override void ApplyEffect()
        {
            ShieldUpSO buff = Buff as ShieldUpSO;
            _player.SetShieldEnabled(true);
        }

        public override void OnFinished()
        {
            if (!Buff.isForever)
            {
                MaxHpUpSO buff = Buff as MaxHpUpSO;
                _player.SetShieldEnabled(false);
                while (_effectStacks > 0)
                {
                    _effectStacks--;
                }
                _player.TriggerDePoweredUpEffect();
            }
        }
    }
}

