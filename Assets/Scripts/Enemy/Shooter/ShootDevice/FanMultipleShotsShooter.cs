using System.Collections;
using UnityEngine;

public class FanMultipleShotsShooter : AutoShootDevice
{
    [SerializeField] PooledBulletProduct _bulletPrefab;
    [SerializeField] Transform _spawnLocation;

    [SerializeField] float _fireRate = 1.0f;
    [SerializeField] int _bulletsPerShot = 5;
    [SerializeField] float _bulletSpreadAngle = 45f;
    [SerializeField] float _delayStart = 0f;
    [SerializeField] bool _shootOnStart = false;

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

        BulletFactory bulletFactory = BulletFactory.Instance;
        if (bulletFactory != null)
        {
            for (int i = 0; i < _bulletsPerShot; i++)
            {
                PooledBulletProduct bulletObject = bulletFactory.CreateBulletProduct(_bulletPrefab.InstanceId);

                if (bulletObject == null)
                {
                    Debug.LogError("AutoSingleShotShooter: created bullet is null");
                    break;
                }

                float angle = startAngle + i * angleStep;
                Quaternion bulletRotation = Quaternion.Euler(0, 0, angle);
                bulletObject.transform.parent = PlaySceneGlobal.Instance.BulletParent;
                bulletObject.transform.position = _spawnLocation.position;
                bulletObject.transform.rotation = bulletRotation;
            }
        }
    }
}