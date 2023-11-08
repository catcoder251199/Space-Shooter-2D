using PlayerNS;
using UnityEngine;
using UnityEngine.Rendering;

namespace Buff
{
    public abstract class SwapShootPattern : TimedBuff
    {
        protected ShootPatternSO _patternBuff;
        protected Player _player;
        protected IShootPattern _grantedPattern;
        public SwapShootPattern(ShootPatternSO buff, GameObject gameObject) : base(buff, gameObject)
        {
            _patternBuff = buff;
            _player = gameObject.GetComponent<Player>();
        }
        public abstract IShootPattern CreateShootPattern();

        protected override void OnActivated()
        {
            _player.TriggerPoweredUpEffect();
            var uiManager = GameManager.Instance?.UIManager;
            if (uiManager != null)
                uiManager.ShowBuffDescriptionPanel(Buff);
        }
        protected override void ApplyEffect()
        {
            var currentPattern = _player.WeaponHandler.CurrentPattern;
            _grantedPattern = CreateShootPattern();
            if (_grantedPattern != null && (_grantedPattern.GetType() != currentPattern.GetType()))
            {
                _player.ChangeShootPattern(_grantedPattern);
            }
        }

        public override void OnFinished()
        {
            if (!Buff.isForever)
            {
                if (_player.WeaponHandler.CurrentPattern == _grantedPattern)
                {
                    _player.ChangeShootPattern(_player.DefaultShootPattern);
                    while (_effectStacks > 0)
                    {
                        // TODO: might decrease something specific to some certain patterns
                        _effectStacks--;
                    }
                    _player.TriggerDePoweredUpEffect();
                }
            }
        }
    }
}