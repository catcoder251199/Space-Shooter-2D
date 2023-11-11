using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PlayerBulletPool : MonoBehaviour
{
    [SerializeField] PlayerBullet _bulletPrefab;

    // Collection checks will throw errors if we try to release an item that is already in the pool.
    [SerializeField] private bool _collectionCheck = true;
    [SerializeField] private int _maxPoolSize = 30;
    [SerializeField] private int _defaultPoolSize = 10;

    private IObjectPool<PlayerBullet> _pool;
    public IObjectPool<PlayerBullet> Pool => _pool;
    public void SetNewBulletPrefab(PlayerBullet bullet)
    {
        _bulletPrefab = bullet;
    }

    private void Awake()
    {
        // Stack_based pool
        _pool = new ObjectPool<PlayerBullet>(
                 CreatePooledBullet,
                 OnTakeFromPool, OnReturnedToPool, OnDestroyPooledObject,
                 _collectionCheck, _defaultPoolSize, _maxPoolSize);
    }

    private PlayerBullet CreatePooledBullet()
    {
        PlayerBullet bulletInstance = Instantiate(_bulletPrefab);
        bulletInstance.bulletPool = _pool;
        return bulletInstance;
    }

    private  void OnTakeFromPool(PlayerBullet bulletInstance)
    {
        bulletInstance.gameObject.SetActive(true);
    }

    private void OnReturnedToPool(PlayerBullet bulletInstance)
    {
        bulletInstance.gameObject.SetActive(false);
    }

    // If the pool capacity is reached then any items returned will be destroyed.
    private void OnDestroyPooledObject(PlayerBullet bulletInstance)
    {
        Destroy(bulletInstance.gameObject);
    }
}
