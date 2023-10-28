using System.Collections;
using UnityEngine;
using UnityEngine.Analytics;

public class SingleShotBehaviour : AttackBehaviour
{
    [SerializeField] private float _fireRate = 1f;
    [SerializeField] private BulletBase _bulletPrefab;
    [SerializeField] private Transform _bulletSpawn;

    private bool shooting = false;
    private Coroutine _shootRoutine;
    private Player _player;
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
        if (_shootRoutine != null)
            StopCoroutine(_shootRoutine);
        shooting = false;
    }

    private IEnumerator ShootCoroutine()
    {
        while (shooting)
        {
            BulletBase bullet = Instantiate(_bulletPrefab, _bulletSpawn.position, Quaternion.identity, PlaySceneGlobal.Instance.BulletParent);
            bullet.DamagableCollider.SetDamage(_player.Damage);
            bullet.DamagableCollider.SetCritRate(_player.CritRate);
            bullet.DamagableCollider.SetCritModifier(_player.CritModifier);
            yield return new WaitForSeconds(_fireRate);
        }
    }
}
