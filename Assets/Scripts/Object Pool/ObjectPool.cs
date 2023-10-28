using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private int _initPoolSize = 0; // The size of the pool
    [SerializeField] private PooledObject _objectToPool; // The object prefab that we want to store

    private Stack<PooledObject> _stack; // The pool itself

    private void Start()
    {
        SetupPool();
    }

    // Create the pool internally (Invoked when the lag is not noticeable)
    private void SetupPool()
    {
        _stack = new Stack<PooledObject>();
        PooledObject pooledInstance = null;
        for (int i = 0; i < _initPoolSize; i++)
        {
            pooledInstance = Instantiate(_objectToPool);
            pooledInstance.Pool = this;
            pooledInstance.gameObject.SetActive(false);
            _stack.Push(pooledInstance);
        }
    }

    // return the first active GameObject from the pool
    public PooledObject GetPooledObject()
    {
        // if the pool is not large enough, instantiate a new PooledObject
        if (_stack.Count <= 0)
        {
            PooledObject newInstance = Instantiate(_objectToPool);
            newInstance.Pool = this;
            newInstance.gameObject.SetActive(true);
            return newInstance;
        }
        PooledObject nextInstance = _stack.Pop();
        nextInstance.gameObject.SetActive(true);
        return nextInstance;
    }

    public void ReturnToPool(PooledObject pooledObject)
    {
        _stack.Push(pooledObject);
        pooledObject.gameObject.SetActive(false);
    }
}
