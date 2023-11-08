using UnityEngine;

namespace Buff
{
    public class BulletSizeBuff : TimedBuff
    {
        private Player _player;
        public BulletSizeBuff(BuffSO buff, GameObject obj) : base(buff, obj)
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
            BulletSizeSO buff = Buff as BulletSizeSO;
            _player.SetBonusBulletScaleWith(buff.scaleInc);
            _player.SetBulletSpeedWith(buff.speedInc);
        }

        public override void OnFinished()
        {
            if (!Buff.isForever)
            {
                BulletSizeSO buff = Buff as BulletSizeSO;
                while (_effectStacks > 0)
                {
                    _player.SetBonusBulletScaleWith(-buff.scaleInc);
                    _player.SetBulletSpeedWith(-buff.speedInc);
                    _effectStacks--;
                }
                _player.TriggerDePoweredUpEffect();
            }
        }
    }
}

