using System.Collections;
using UnityEngine;

public class FanMultipleShotsShooter : AutoShootDevice
{
    [SerializeField] EnemyBulletBase _bulletPrefab;
    [SerializeField] Transform _spawnLocation;

    [SerializeField] float _fireRate = 1.0f;
    [SerializeField] int _bulletsPerShot = 5;
    [SerializeField] float _bulletSpreadAngle = 45f;

    [SerializeField] float _delayStart = 0f;
    [SerializeField] bool _shootOnStart = false;

    [SerializeField] ObjectPool _bulletPool;

    private bool shooterEnabled = false;
    private Coroutine _shootRoutine;

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

    public override float GetDelayStart()
    {
        return _delayStart;
    }

    private IEnumerator ShootCoroutine()
    {
        yield return new WaitForSeconds(_delayStart);

        while (shooterEnabled)
        {
            FireFanShot();
            yield return new WaitForSeconds(_fireRate);
        }
    }

    private void FireFanShot()
    {
        float angleStep = _bulletSpreadAngle / (_bulletsPerShot - 1);
        float startAngle = transform.rotation.eulerAngles.z - _bulletSpreadAngle / 2;
        for (int i = 0; i < _bulletsPerShot; i++)
        {
            GameObject bulletObject = _bulletPool.GetPooledObject().gameObject;

            if (bulletObject == null)
                continue;

            float angle = startAngle + i * angleStep;
            Quaternion bulletRotation = Quaternion.Euler(0, 0, angle);
            bulletObject.transform.parent = PlaySceneGlobal.Instance.BulletParent;
            bulletObject.transform.position = _spawnLocation.position;
            bulletObject.transform.rotation = bulletRotation;
        }
    }
}