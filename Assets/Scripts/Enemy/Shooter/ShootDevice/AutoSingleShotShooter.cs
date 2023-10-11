using System.Collections;
using UnityEngine;

public class AutoSingleShotShooter : AutoShootDevice
{
    [SerializeField] BulletBase _bulletPrefab;
    [SerializeField] Transform _spawnLocation;
    [SerializeField] float _fireRate = 1.0f;
    [SerializeField] float _delay = 0f;
    [SerializeField] bool _shootOnStart = false;
    [SerializeField] int _damage = 1;

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
        while (shooterEnabled)
        {
            yield return new WaitForSeconds(_delay);
            Instantiate(_bulletPrefab, _spawnLocation.position, gameObject.transform.rotation);
            yield return new WaitForSeconds(_fireRate);
        }
    }
}