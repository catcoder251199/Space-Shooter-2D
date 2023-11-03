using System.Collections;
using UnityEngine;

namespace PlayerNS
{
    public class DoubleShotPattern : IShootPattern
    {
        private WeaponHandler _weaponHandler;
        private Coroutine _shootRoutine;
        private float _baseFireRate = 1.2f;
        private float _baseBulletSpeed = 15f;

        public float GetFireRate()
        {
            return Mathf.Max(_baseFireRate - 0.15f * _weaponHandler.FireRateStack, 0.1f);
        }

        public float GetBulletSpeed()
        {
            return _baseBulletSpeed;
        }

        public DoubleShotPattern(WeaponHandler weaponHandler)
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
                _weaponHandler.StopCoroutine(_shootRoutine);
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
            Transform leftBulletSpawn = _weaponHandler.BulletLeftSpawn;
            Transform rightBulletSpawn = _weaponHandler.BulletRightSpawn;

            while (true)
            {
                bool leftSpawned = false;
                for (int i = 0; i < 2; i++)
                {
                    PlayerBullet bullet = pool?.Pool.Get();
                    if (bullet != null)
                    {
                        bool isCritical = false;
                        bullet.Damage = player.DamageHandler.GetCalculatedDamage(out isCritical);
                        bullet.IsCritical = isCritical;
                        bullet.transform.position = leftSpawned ? rightBulletSpawn.position : leftBulletSpawn.position;
                        leftSpawned = !leftSpawned;
                        bullet.transform.rotation = Quaternion.identity;
                        bullet.transform.parent = PlaySceneGlobal.Instance.BulletParent;
                        bullet.Speed = GetBulletSpeed();
                    }
                    else
                        Debug.LogError("DoubleShotPattern: Pooled Bullet is null !");
                }
                yield return new WaitForSeconds(GetFireRate());
            }
        }
    }
}
