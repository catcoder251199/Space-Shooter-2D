using UnityEngine;

namespace Buff
{
    public class AttackUpBuff : TimedBuff
    {
        private Player _player;
        public AttackUpBuff(BuffSO buff, GameObject obj) : base(buff, obj)
        {
            _player = obj.GetComponent<Player>();
        }

        protected override void ApplyEffect()
        {
            AttackUpSO attackUpBuff = Buff as AttackUpSO;
            _player.SetAttackUpWith(attackUpBuff.attackIncrease);
            _player.TriggerPoweredUpEffect();
            var uiManager = GameManager.Instance?.UIManager;
            if (uiManager != null)
                uiManager.ShowBuffDescriptionPanel(attackUpBuff);
        }

        public override void OnFinished()
        {
            if (!Buff.isForever)
            {
                AttackUpSO attackUpBuff = Buff as AttackUpSO;
                while (_effectStacks > 0)
                {
                    _player.SetAttackUpWith(-attackUpBuff.attackIncrease);
                    _player.TriggerDePoweredUpEffect();
                    _effectStacks--;
                }
            }
        }
    }
}

