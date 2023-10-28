using System.Collections;
using UnityEngine;

public class AutoDoubleShotShooter : AutoShootDevice
{
    [SerializeField] EnemyBulletBase _bulletPrefab;
    [SerializeField] Transform[] _spawnLocation;
    [SerializeField] float _fireRate = 1.0f;
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
            for (int i = 0; i < 2; i++)
            {
                GameObject bulletObject = _bulletPool.GetPooledObject().gameObject;

                if (bulletObject == null)
                    continue;

                bulletObject.transform.parent = PlaySceneGlobal.Instance.BulletParent;
                bulletObject.transform.position = _spawnLocation[i].position;
                bulletObject.transform.rotation = this.transform.rotation;
            }

            yield return new WaitForSeconds(_fireRate);
        }
    }
}