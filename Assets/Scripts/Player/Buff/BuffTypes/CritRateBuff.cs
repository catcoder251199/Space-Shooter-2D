using UnityEngine;

namespace Buff
{
    public class CritRateBuff : TimedBuff
    {
        private Player _player;
        public CritRateBuff(BuffSO buff, GameObject obj) : base(buff, obj)
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
            CritRateSO buff = Buff as CritRateSO;
            _player.SetCritRateWith(buff.critRateIncrease);
        }

        public override void OnFinished()
        {
            if (!Buff.isForever)
            {
                CritRateSO buff = Buff as CritRateSO;
                while (_effectStacks > 0)
                {
                    _player.SetCritRateWith(-buff.critRateIncrease);
                    _effectStacks--;
                }
                _player.TriggerDePoweredUpEffect();
            }
        }
    }
}

