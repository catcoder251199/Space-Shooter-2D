using System.Collections;
using UnityEngine;

namespace PlayerNS
{
    public class SplitShotPattern : IShootPattern
    {
        private WeaponHandler _weaponHandler;
        private Coroutine _shootRoutine;
        private float _bulletSpreadAngle = 30f;
        private int _bulletsPerShot = 3;
        private float _baseFireRate = 1.2f;
        private float _baseBulletSpeed = 15f;

        public float GetFireRate()
        {
            return Mathf.Max(_baseFireRate - 0.1f * _weaponHandler.FireRateStack, 0.1f);
        }

        public float GetBulletSpeed()
        {
            return _baseBulletSpeed;
        }

        public SplitShotPattern(WeaponHandler weaponHandler)
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
            while (true)
            {
                FireFanShot();
                yield return new WaitForSeconds(GetFireRate());
            }
        }

        private void FireFanShot()
        {
            Transform centerSpawn = _weaponHandler.BulletCenterSpawn;
            Player player = _weaponHandler.Player;
            PlayerBulletPool pool = _weaponHandler.Player?.BulletPool;
            float angleStep = _bulletSpreadAngle / (_bulletsPerShot - 1);
            float startAngle = centerSpawn.rotation.eulerAngles.z - _bulletSpreadAngle / 2;

            for (int i = 0; i < _bulletsPerShot; i++)
            {
                PlayerBullet bullet = pool?.Pool.Get();
                if (bullet != null)
                {
                    bool isCritical = false;
                    bullet.Damage = player.DamageHandler.GetCalculatedDamage(out isCritical);
                    bullet.IsCritical = isCritical;

                    float angle = startAngle + i * angleStep;
                    Quaternion bulletRotation = Quaternion.Euler(0, 0, angle);
                    bullet.transform.rotation = bulletRotation;
                    bullet.transform.position = centerSpawn.position;
                    bullet.transform.parent = PlaySceneGlobal.Instance.BulletParent;
                    bullet.Speed = GetBulletSpeed();
                }
                else
                    Debug.LogError("SplitShotPattern: Pooled Bullet is null !");
            }
        }
    }
}
