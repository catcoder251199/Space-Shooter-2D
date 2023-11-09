using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class SpawnableFactory : MonoBehaviour
{
    [SerializeField, Tooltip("* Add anything/any enemy can be spawned in this level to this list")] 
    PooledSpawnableProduct[] _prefabList; // list of pre-stored prefabs 
    [SerializeField, Header("Spawnable Object Pools")] private int _defaultCapacity; // initial size of pool
    [SerializeField] private int _maxSize = 50;
    [SerializeField] private bool _checkExistenceInPool = true; // throw an exception if we try to return an existing item, already in the pool
    
    private Dictionary<int, PooledSpawnableProduct> _prefabDict; // dictionary of pre-stored prefabs 
    private Dictionary<int, IObjectPool<PooledSpawnableProduct>> _poolDict; // dictionary of different pools each contains corresponding prefabs 

    private void Awake()
    {
        SetupFactory();
    }

    private void SetupFactory()
    {
        _prefabDict = new Dictionary<int, PooledSpawnableProduct>();
        _poolDict = new Dictionary<int, IObjectPool<PooledSpawnableProduct>>();

        foreach (PooledSpawnableProduct prefab in _prefabList)
        {
            int prefabId = prefab.InstanceId;

            if (!_poolDict.ContainsKey(prefabId))
            {
                _prefabDict.Add(prefabId, prefab);

                var addedPool =
                    new ObjectPool<PooledSpawnableProduct>(
                    () => CreateSpawnableObject(prefabId),
                    OnGetFromPool, OnReleaseToPool, OnDestroyPooledObject,
                    _checkExistenceInPool, _defaultCapacity, _maxSize);

                _poolDict.Add(prefabId, addedPool);
            }
            else
                Debug.LogError("Duplicate prefab instance id");
        }
    }

    public PooledSpawnableProduct CreateSpawnableProduct(int instanceId)
    {
        PooledSpawnableProduct retInstance = null;
        IObjectPool<PooledSpawnableProduct> pool = GetPool(instanceId);
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

    private PooledSpawnableProduct GetPrefab(int prefabId)
    {
        PooledSpawnableProduct ret = null;
        if (_prefabDict.ContainsKey(prefabId))
            ret = _prefabDict[prefabId];
        return ret;
    }
    
    private IObjectPool<PooledSpawnableProduct> GetPool(int instanceId)
    {
        IObjectPool<PooledSpawnableProduct> pool = null;
        if (_poolDict.ContainsKey(instanceId))
            pool = _poolDict[instanceId];
        return pool;
    }


    //---Object Pool---
    // invoked when creating an item to populate the object pool
    private PooledSpawnableProduct CreateSpawnableObject(int prefabId)
    {
        PooledSpawnableProduct prefab = GetPrefab(prefabId);
        PooledSpawnableProduct prefabInstance = Instantiate(prefab);
        prefabInstance.SpawnPool = GetPool(prefabId);
        return prefabInstance;
    }

    // invoked when returning an item to the object pool
    private void OnReleaseToPool(PooledSpawnableProduct pooledObject)
    {
        pooledObject.gameObject.SetActive(false);
    }

    // invoked when retrieving the next item from the object pool
    private void OnGetFromPool(PooledSpawnableProduct pooledObject)
    {
        pooledObject.gameObject.SetActive(true);
    }

    // invoked when we exceed the maximum number of pooled items (i.e. destroy the pooled object)
    private void OnDestroyPooledObject(PooledSpawnableProduct pooledObject)
    {
        Destroy(pooledObject.gameObject);
    }
    //---

    public int GetRandomInstanceIdInPool()
    {
        int idx = Random.Range(0, _prefabList.Length);
        return _prefabList[idx].InstanceId;
    }
}
