using System.Collections;
using UnityEngine;
using UnityEngine.Analytics;

public class SingleShotBehaviour : AttackBehaviour
{
    [SerializeField] private float _fireRate = 1f;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _bulletSpawn;

    private bool shooting = false;
    private Coroutine _shootRoutine;
    [SerializeField] private Player _player;
    private SingleShotBehaviourSO _behaviourData;

    private void Awake()
    {
        _player = gameObject.GetComponent<Player>();
        _behaviourData = _player.GetAttackBehaviourData(Player.AttackBehaviourType.Single) as SingleShotBehaviourSO;
        Init();
    }

    private void Init()
    {
        _fireRate = _behaviourData.fireRate;
        _bulletPrefab = _player.GetBulletPrefab();
        _bulletSpawn = transform.Find("SingleShot/BulletSpawn");
    }

    public override void StartDoing() 
    {
        if (!shooting)
        {
            shooting = true;
            _shootRoutine = StartCoroutine(ShootCoroutine());
        }
    }
    public override void StopDoing() 
    {
        StopCoroutine(_shootRoutine);
    }

    private IEnumerator ShootCoroutine()
    {
        while (shooting)
        {
            Instantiate(_player.GetBulletPrefab(), _bulletSpawn.position, Quaternion.identity);
            yield return new WaitForSeconds(_fireRate);
        }
    }
}
