using UnityEngine;

namespace Buff
{
    public class FiringRateBuff : TimedBuff
    {
        private Player _player;
        public FiringRateBuff(BuffSO buff, GameObject obj) : base(buff, obj)
        {
            _player = obj.GetComponent<Player>();
        }

        protected override void ApplyEffect()
        {
            FiringRateSO buff = Buff as FiringRateSO;
            _player.SetFiringRateStackWith(buff.firingStackInc);
            _player.TriggerPoweredUpEffect();
            var uiManager = GameManager.Instance?.UIManager;
            if (uiManager != null)
                uiManager.ShowBuffDescriptionPanel(buff);
        }

        public override void OnFinished()
        {
            if (!Buff.isForever)
            {
                FiringRateSO buff = Buff as FiringRateSO;
                while (_effectStacks > 0)
                {
                    _player.SetFiringRateStackWith(-buff.firingStackInc);
                    _player.TriggerDePoweredUpEffect();
                    _effectStacks--;
                }
            }
        }
    }
}

