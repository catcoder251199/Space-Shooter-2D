using UnityEngine;

namespace Buff
{
    public class MaxHpUpBuff : TimedBuff
    {
        private Player _player;
        public MaxHpUpBuff(BuffSO buff, GameObject obj) : base(buff, obj)
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
            MaxHpUpSO buff = Buff as MaxHpUpSO;
            _player.SetHpMaxWith(buff.incAmount);
        }

        public override void OnFinished()
        {
            if (!Buff.isForever)
            {
                MaxHpUpSO buff = Buff as MaxHpUpSO;
                while (_effectStacks > 0)
                {
                    _player.SetHpMaxWith(-buff.incAmount);
                    _effectStacks--;
                }
                _player.TriggerDePoweredUpEffect();
            }
        }
    }
}

