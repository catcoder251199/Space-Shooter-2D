using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;

public class PooledSpawnableProduct : MonoBehaviour, ISpawnableProduct // or IProduct
{
    public UnityEvent onInitializeInvoked;
    public event Action<PooledSpawnableProduct> onSpawnableObjectDestroyed;

    public IObjectPool<PooledSpawnableProduct> SpawnPool { get; set; }
    public int InstanceId { get => gameObject.GetInstanceID(); }
    public void Initialize()
    {
        onInitializeInvoked?.Invoke();
    }

    public void Release()
    {
        if (SpawnPool != null)
            SpawnPool.Release(this);
        else 
            Destroy(gameObject);
    }

    public void OnDisable()
    {
        onSpawnableObjectDestroyed?.Invoke(this);
    }
}
