using System.Collections;
using UnityEngine;
using UnityEngine.Analytics;

public class SingleShotBehaviour : AttackBehaviour
{
    [SerializeField] private float _fireRate = 1f;
    [SerializeField] private Transform _bulletSpawn;

    private bool shooting = false;
    private Coroutine _shootRoutine;
    private Player _player;
    private SingleShotBehaviourSO _behaviourData;

    private void Awake()
    {
        _player = gameObject.GetComponent<Player>();
        //_behaviourData = _player.GetAttackBehaviourData(Player.AttackBehaviourType.Single) as SingleShotBehaviourSO;
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
            PlayerBullet bullet = _player?.BulletPool?.Pool.Get();
            if (bullet != null)
            {
                bool isCritical = false;
                bullet.Damage = _player.DamageHandler.GetCalculatedDamage(out isCritical);
                bullet.IsCritical = isCritical;
                bullet.transform.position = _bulletSpawn.position;
                bullet.transform.rotation = Quaternion.identity;
                bullet.transform.parent = PlaySceneGlobal.Instance.BulletParent;
            }
            else
            {
                Debug.LogError("SingleShotBehaviour: Pooled Bullet is null !");
            }
            Debug.Log("Fire bullet");
            yield return new WaitForSeconds(_fireRate);
        }
    }
}
