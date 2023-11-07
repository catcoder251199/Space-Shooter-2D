using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace PlayerNS
{
    public class ShotgunPattern : IShootPattern
    {
        private WeaponHandler _weaponHandler;
        private Coroutine _shootRoutine;
        private float _bulletSpreadAngle = 45f;
        private int _bulletsPerShot = 5;
        private int _splitNumber = 8;
        private float _baseFireRate = 1.6f;
        private float _baseBulletSpeed = 15f;

        public float GetFireRate()
        {
           return Mathf.Max(_baseFireRate - 0.2f * _weaponHandler.FireRateStack, 0.1f);
        }

        public float GetBulletSpeed()
        {
            return _baseBulletSpeed + 2f * _weaponHandler.SpeedStack;
        }

        public ShotgunPattern(WeaponHandler weaponHandler)
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

            float angleStep = _bulletSpreadAngle / (_splitNumber - 1);
            float startAngle = centerSpawn.rotation.eulerAngles.z - _bulletSpreadAngle / 2;
            // We create a random subset of set [0, 1, ... _splitNumber - 1]
            var indicesSet = GetRandomSubset(Enumerable.Range(0, _splitNumber).ToList(), _bulletsPerShot);
            foreach (var index in indicesSet)
            {
                PlayerBullet bullet = pool?.Pool.Get();
                if (bullet != null)
                {
                    bool isCritical = false;
                    bullet.Damage = player.DamageHandler.GetCalculatedDamage(out isCritical);
                    bullet.IsCritical = isCritical;

                    float angle = startAngle + index * angleStep;
                    Quaternion bulletRotation = Quaternion.Euler(0, 0, angle);
                    bullet.transform.rotation = bulletRotation;
                    bullet.transform.position = centerSpawn.position;
                    bullet.transform.parent = PlaySceneGlobal.Instance.BulletParent;
                    bullet.Speed = GetBulletSpeed();
                    bullet.transform.localScale = Vector2.one * (1 + _weaponHandler.BonusScale);
                }
                else
                    Debug.LogError("ShotgunPattern: Pooled Bullet is null !");
            }
        }

        private static HashSet<int> GetRandomSubset(List<int> indicesList, int returnedSetSize)
        {
            if (returnedSetSize > indicesList.Count)
                returnedSetSize = indicesList.Count;

            HashSet<int> randomSubset = new HashSet<int>();

            for (int i = 0; i < returnedSetSize; i++) // O(m)
            {
                int randomIndex = Random.Range(0, indicesList.Count); // 0(1)
                randomSubset.Add(indicesList[randomIndex]); // O(1)
                indicesList.RemoveAt(randomIndex); // O(n)
            }

            return randomSubset; // Final O(m * n)
        }
    }
}
