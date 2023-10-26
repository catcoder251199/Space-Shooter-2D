using System.Collections;
using UnityEngine;

public class AutoSingleShotShooter : AutoShootDevice
{
    [SerializeField] EnemyBulletBase _bulletPrefab;
    [SerializeField] Transform _spawnLocation;

    [SerializeField] float _fireRate = 1.0f;
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
            Instantiate(_bulletPrefab, _spawnLocation.position, gameObject.transform.rotation, PlaySceneGlobal.Instance.BulletParent);
            yield return new WaitForSeconds(_fireRate);
        }
    }
}