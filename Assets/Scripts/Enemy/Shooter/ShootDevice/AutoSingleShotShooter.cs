﻿using System.Collections;
using UnityEngine;

public class AutoSingleShotShooter : AutoShootDevice
{
    [SerializeField] BulletBase _bulletPrefab;
    [SerializeField] Transform _spawnLocation;

    [SerializeField] float _fireRate = 1.0f;
    [SerializeField] float _delayStart = 0f;
    [SerializeField] bool _shootOnStart = false;

    [SerializeField, Header("Bullet")] int _damage = 1;
    [SerializeField] int _bulletSpeed = 1;

    private bool shooterEnabled = false;
    private Coroutine _shootRoutine;

    public int Damage => _damage;

    private void Start()
    {
        if (_shootOnStart)
        {
            Activate();
        }
    }

    public override bool IsActivate()
    {
        return shooterEnabled;
    }

    public override void Activate()
    {
        if (!shooterEnabled)
        {
            shooterEnabled = true;
            _shootRoutine = StartCoroutine(ShootCoroutine());
        }
    }
    public override void Deactivate()
    {
        StopCoroutine(_shootRoutine);
        shooterEnabled = false;
    }

    private IEnumerator ShootCoroutine()
    {
        yield return new WaitForSeconds(_delayStart);

        while (shooterEnabled)
        {
            BulletBase bullet = Instantiate(_bulletPrefab, _spawnLocation.position, gameObject.transform.rotation, PlaySceneGlobal.Instance.BulletParent);
            bullet.DamagableCollider.SetDamage(_damage);
            bullet.Speed = _bulletSpeed;
            yield return new WaitForSeconds(_fireRate);
        }
    }
}