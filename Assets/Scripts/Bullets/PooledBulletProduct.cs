using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;

public class PooledBulletProduct : MonoBehaviour
{
    public UnityEvent onInitializeInvoked;

    public IObjectPool<PooledBulletProduct> SpawnPool { get; set; }
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
}
