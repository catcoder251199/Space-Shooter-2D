using UnityEngine;

namespace Buff
{
    public class CritDamageBuff : TimedBuff
    {
        private Player _player;
        public CritDamageBuff(BuffSO buff, GameObject obj) : base(buff, obj)
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
            CritDamageSO buff = Buff as CritDamageSO;
            _player.SetCritDamageModifierWith(buff.critModifier);
        }

        public override void OnFinished()
        {
            if (!Buff.isForever)
            {
                CritDamageSO buff = Buff as CritDamageSO;
                while (_effectStacks > 0)
                {
                    _player.SetCritDamageModifierWith(-buff.critModifier);
                    _effectStacks--;
                }
                _player.TriggerDePoweredUpEffect();
            }
        }
    }
}

