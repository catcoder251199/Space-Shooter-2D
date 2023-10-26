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
            float angle = startAngle + i * angleStep;
            Quaternion bulletRotation = Quaternion.Euler(0, 0, angle);
            Instantiate(_bulletPrefab, _spawnLocation.position, bulletRotation, PlaySceneGlobal.Instance.BulletParent);
        }
    }
}