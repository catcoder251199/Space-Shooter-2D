using System.Collections;
using UnityEngine;

namespace PlayerNS
{
    public class SingleShotPattern : IShootPattern
    {
        private WeaponHandler _weaponHandler;
        private Coroutine _shootRoutine;
        private float _baseFireRate = 0.8f;
        private float _baseBulletSpeed = 15f;


        public float GetFireRate()
        {
            return Mathf.Max(_baseFireRate - 0.08f * _weaponHandler.FireRateStack, 0.1f);
        }
        public float GetBulletSpeed()
        {
            return _baseBulletSpeed + 2f * _weaponHandler.SpeedStack;
        }

        public SingleShotPattern(WeaponHandler weaponHandler)
        {
            _weaponHandler = weaponHandler;
        }

        public void Start()
        {
            if (!IsShooting())
                _shootRoutine = _weaponHandler.StartCoroutine(ShootCoroutine());
        }

        public void Stop()
        {
            if (IsShooting())
            {
                _weaponHandler.StopCoroutine(_shootRoutine);
                _shootRoutine = null;
            }
        }

        public void OnRemoved() { Stop(); }

        public void OnAdded() { }

        public bool IsShooting()
        {
            return _shootRoutine != null;
        }

        private IEnumerator ShootCoroutine()
        {
            PlayerBulletPool pool = _weaponHandler.Player?.BulletPool;
            Player player = _weaponHandler.Player;
            Transform centerBulletSpawn = _weaponHandler.BulletCenterSpawn;

            while (true)
            {
                PlayerBullet bullet = pool?.Pool.Get();
                if (bullet != null)
                {
                    bool isCritical = false;
                    bullet.Damage = player.DamageHandler.GetCalculatedDamage(out isCritical);
                    bullet.IsCritical = isCritical;
                    bullet.transform.position = centerBulletSpawn.position;
                    bullet.transform.rotation = Quaternion.identity;
                    bullet.transform.localScale = Vector2.one * (1 + _weaponHandler.BonusScale);
                    bullet.transform.parent = PlaySceneGlobal.Instance.BulletParent;
                    bullet.Speed = GetBulletSpeed();

                    SoundManager.Instance.PlayEffectOneShot(_weaponHandler.ShootSound);
                }
                else
                    Debug.LogError("SingleShotPattern: Pooled Bullet is null !");
                yield return new WaitForSeconds(GetFireRate());
            }
        }

    }
}
