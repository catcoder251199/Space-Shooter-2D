using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class SingleShotBehaviour : Enemy.ShootingBehaviour
    {
        [SerializeField] private float _waitForNextFire = 1f;
        [SerializeField] private GameObject _bulletPrefab;
        [SerializeField] private Transform _bulletSpawn;

        private bool shooting = false;
        private Coroutine _shootRoutine;

        public override void StartAttack()
        {
            if (!shooting)
            {
                shooting = true;
                _shootRoutine = StartCoroutine(ShootCoroutine());
            }
        }
        public override void EndAttack()
        {
            StopCoroutine(_shootRoutine);
            shooting = false;
        }

        private IEnumerator ShootCoroutine()
        {
            while (shooting)
            {
                Instantiate(_bulletPrefab, _bulletSpawn.position, gameObject.transform.rotation);
                yield return new WaitForSeconds(_waitForNextFire);
            }
        }
    }
}

