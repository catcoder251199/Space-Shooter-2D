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

        protected override void ApplyEffect()
        {
            CritDamageSO buff = Buff as CritDamageSO;
            _player.SetCritDamageModifierWith(buff.critModifier);
            _player.TriggerPoweredUpEffect();
            var uiManager = GameManager.Instance?.UIManager;
            if (uiManager != null)
                uiManager.ShowBuffDescriptionPanel(buff);
        }

        public override void OnFinished()
        {
            if (!Buff.isForever)
            {
                CritDamageSO buff = Buff as CritDamageSO;
                while (_effectStacks > 0)
                {
                    _player.SetCritDamageModifierWith(-buff.critModifier);
                    _player.TriggerDePoweredUpEffect();
                    _effectStacks--;
                }
            }
        }
    }
}

