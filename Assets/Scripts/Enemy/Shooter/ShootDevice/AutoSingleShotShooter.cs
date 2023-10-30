using System.Collections;
using UnityEngine;

public class AutoSingleShotShooter : AutoShootDevice
{
    [SerializeField] PooledBulletProduct _bulletPrefab;
    [SerializeField] Transform _spawnLocation;
    [SerializeField] float _fireRate = 1.0f;
    [SerializeField] float _delayStart = 0f;
    [SerializeField] bool _shootOnStart = false;

    private bool _shooterEnabled = false;
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
        return _shooterEnabled;
    }

    public override void Activate()
    {
        if (!_shooterEnabled)
        {
            _shooterEnabled = true;
            _shootRoutine = StartCoroutine(ShootCoroutine());
        }
    }
    public override void Deactivate()
    {
        StopCoroutine(_shootRoutine);
        _shooterEnabled = false;
    }

    public override float GetDelayStart()
    {
        return _delayStart;
    }

    private IEnumerator ShootCoroutine()
    {
        yield return new WaitForSeconds(_delayStart);

        BulletFactory bulletFactory = BulletFactory.Instance;
        if (bulletFactory != null)
        {
            while (_shooterEnabled)
            {
                PooledBulletProduct bulletObject = bulletFactory.CreateBulletProduct(_bulletPrefab.InstanceId);

                if (bulletObject == null)
                {
                    Debug.LogError("AutoSingleShotShooter: created bullet is null");
                    break;
                }

                bulletObject.transform.parent = PlaySceneGlobal.Instance.BulletParent;
                bulletObject.transform.position = this.transform.position;
                bulletObject.transform.rotation = this.transform.rotation;
                yield return new WaitForSeconds(_fireRate);
            }

        }
    }
}