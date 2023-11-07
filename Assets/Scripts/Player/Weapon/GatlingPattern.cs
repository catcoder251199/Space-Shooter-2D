using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Random = UnityEngine.Random;

namespace PlayerNS
{
    public class GatlingPattern : IShootPattern
    {
        private WeaponHandler _weaponHandler;
        private Coroutine _shootRoutine;
        private float _bulletSpreadAngle = 20f;
        private float _baseFireRate = 0.3f;
        private float _baseBulletSpeed = 18f;

        public float GetFireRate()
        {
           return Mathf.Max(_baseFireRate - 0.08f * _weaponHandler.FireRateStack, 0.05f);
        }
        public float GetBulletSpeed()
        {
            return _baseBulletSpeed + 2f * _weaponHandler.SpeedStack;
        }

        public GatlingPattern(WeaponHandler weaponHandler)
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
            Transform centerSpawn = _weaponHandler.BulletCenterSpawn;
            float halfAngle = _bulletSpreadAngle / 2;
            float startAngle = centerSpawn.rotation.eulerAngles.z - halfAngle;
            float endAngle = centerSpawn.rotation.eulerAngles.z + halfAngle;
            Player player = _weaponHandler.Player;
            PlayerBulletPool pool = _weaponHandler.Player?.BulletPool;

            while (true)
            {
                PlayerBullet bullet = pool?.Pool.Get();
                if (bullet != null)
                {
                    bool isCritical = false;
                    bullet.Damage = player.DamageHandler.GetCalculatedDamage(out isCritical);
                    bullet.IsCritical = isCritical;

                    float angle = Random.Range((int) startAngle, (int) endAngle);
                    Quaternion bulletRotation = Quaternion.Euler(0, 0, angle);
                    bullet.transform.rotation = bulletRotation;
                    bullet.transform.position = centerSpawn.position;
                    bullet.transform.parent = PlaySceneGlobal.Instance.BulletParent;
                    bullet.Speed = GetBulletSpeed();
                    bullet.transform.localScale = Vector2.one * (1 + _weaponHandler.BonusScale);
                }
                else
                    Debug.LogError("GatlingPattern: Pooled Bullet is null !");

                yield return new WaitForSeconds(GetFireRate());
            }
        }
    }
}
