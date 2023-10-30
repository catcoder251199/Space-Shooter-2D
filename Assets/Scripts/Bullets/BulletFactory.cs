using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

// Singleton Class
public class BulletFactory : MonoBehaviour
{
    public static BulletFactory Instance;

    [SerializeField, Tooltip("* Add any bullet can be spawned in this level to this list")]
    PooledBulletProduct[] _prefabList; // list of pre-stored prefabs 

    [SerializeField, Header("Bullet Object Pools")] private int _defaultCapacity; // initial size of pool
    [SerializeField] private int _maxSize = 100;
    [SerializeField] private bool _checkExistenceInPool = true; // throw an exception if we try to return an existing item, already in the pool

    private Dictionary<int, PooledBulletProduct> _prefabDict; // dictionary of pre-stored prefabs 
    private Dictionary<int, IObjectPool<PooledBulletProduct>> _poolDict; // dictionary of different pools each contains corresponding prefabs 

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            SetupFactory();
        }
    }

    private void SetupFactory()
    {
        _prefabDict = new Dictionary<int, PooledBulletProduct>();
        _poolDict = new Dictionary<int, IObjectPool<PooledBulletProduct>>();

        foreach (PooledBulletProduct prefab in _prefabList)
        {
            int prefabId = prefab.InstanceId;

            if (!_poolDict.ContainsKey(prefabId))
            {
                _prefabDict.Add(prefabId, prefab);

                var addedPool =
                    new ObjectPool<PooledBulletProduct>(
                    () => CreateSpawnableObject(prefabId),
                    OnGetFromPool, OnReleaseToPool, OnDestroyPooledObject,
                    _checkExistenceInPool, _defaultCapacity, _maxSize);

                _poolDict.Add(prefabId, addedPool);
            }
            else
                Debug.LogError("Duplicate prefab instance id");
        }
    }

    public PooledBulletProduct CreateBulletProduct(int instanceId)
    {
        PooledBulletProduct retInstance = null;
        IObjectPool<PooledBulletProduct> pool = GetPool(instanceId);
        if (pool != null)
        {
            retInstance = pool.Get();

            if (retInstance != null)
            {
                retInstance.Initialize();
            }

        }
        return retInstance;
    }

    private PooledBulletProduct GetPrefab(int prefabId)
    {
        PooledBulletProduct ret = null;
        if (_prefabDict.ContainsKey(prefabId))
            ret = _prefabDict[prefabId];
        return ret;
    }

    private IObjectPool<PooledBulletProduct> GetPool(int instanceId)
    {
        IObjectPool<PooledBulletProduct> pool = null;
        if (_poolDict.ContainsKey(instanceId))
            pool = _poolDict[instanceId];
        return pool;
    }


    //---Object Pool---
    // invoked when creating an item to populate the object pool
    private PooledBulletProduct CreateSpawnableObject(int prefabId)
    {
        PooledBulletProduct prefab = GetPrefab(prefabId);
        PooledBulletProduct prefabInstance = Instantiate(prefab);
        prefabInstance.SpawnPool = GetPool(prefabId);
        return prefabInstance;
    }

    // invoked when returning an item to the object pool
    private void OnReleaseToPool(PooledBulletProduct pooledObject)
    {
        pooledObject.gameObject.SetActive(false);
    }

    // invoked when retrieving the next item from the object pool
    private void OnGetFromPool(PooledBulletProduct pooledObject)
    {
        pooledObject.gameObject.SetActive(true);
    }

    // invoked when we exceed the maximum number of pooled items (i.e. destroy the pooled object)
    private void OnDestroyPooledObject(PooledBulletProduct pooledObject)
    {
        Destroy(pooledObject.gameObject);
    }
    //---
}
