using System.Collections;
using System.Collections.Generic;
using UnityEngine.Pool;
using UnityEngine;
using static WaveSO;
using UnityEngine.Events;
using System;
using JetBrains.Annotations;

public class SpawnManager : MonoBehaviour
{
    public event Action<int, int> onDamageableSpawnableCountChanged; // from: int, to: int
    public UnityEvent onCurrentWaveFinished;

    [SerializeField] private WaveSO[] _waveSOList;
    [SerializeField] private int _currentWave;

    private Coroutine _spawnCoroutine;
    public int CurrentWave => _currentWave;
    public int wavesTotal => _waveSOList.Length;
    public int FirstWave => 0;

    [SerializeField] SpawnableFactory _factory;
    private HashSet<PooledSpawnableProduct> _activeObjects = new HashSet<PooledSpawnableProduct>();
    private bool _canSpawnMoreInCurrentWave = false;

    private int _damageableSpawnableCount = 0;
    public int DamageableSpawnableCount
    {
        private set
        {
            if (_damageableSpawnableCount != value)
            {
                if (value <= 0)
                    value = 0;
                onDamageableSpawnableCountChanged(_damageableSpawnableCount, value);
                _damageableSpawnableCount = value;
            }
        }
        get { return _damageableSpawnableCount;}
    }

    public bool CanStartSpawn()
    {
        return _spawnCoroutine == null;
    }


    public void StartSpawn()
    {
        if (CanStartSpawn())
            _spawnCoroutine = StartCoroutine(SpawnRoutine());
    }

    public bool HaveNextWave()
    {
        return _currentWave + 1 < _waveSOList.Length;
    }

    public void MoveToNextWave()
    {
        _currentWave = Mathf.Clamp(_currentWave + 1, 0, _waveSOList.Length - 1);
    }

    public void SpawnNext()
    {
        if (HaveNextWave())
        {
            _currentWave++;
            StartSpawn();
        }
    }


    private IEnumerator SpawnRoutine()
    {
        WaveSO waveSO = _waveSOList[_currentWave];
        SpawnedObject[] spawnedList = waveSO.SpawnedObjectsList;
        int spawnedCount = spawnedList.Length;
        _canSpawnMoreInCurrentWave = true;
        for (int i = 0; i < spawnedCount; i++)
        {
            int enemyCount = spawnedList[i].count;
            for (int j = 0; j < enemyCount; j++)
            {
                var spawnableObject = _factory.CreateSpawnableProduct(spawnedList[i].prefab.GetInstanceID());
                if (spawnableObject == null)
                {
                    Debug.LogError("SpawnManager.SpawnRoutine: Get a null spawnable Object");
                    continue;
                }
                spawnableObject.transform.parent = PlaySceneGlobal.Instance.SpawnedObjectParent;

                _activeObjects.Add(spawnableObject);
                Debug.Log($"SpawnManager.ActiveObject.Spawn: {_activeObjects.Count}");

                spawnableObject.onSpawnableObjectDestroyed += OnOneSpanwableTerminated;

                if (spawnableObject.GetComponent<Health>() != null)
                    DamageableSpawnableCount += 1;

                if (i == spawnedCount - 1 && j == enemyCount - 1) // If we spawn the last one
                {
                    _canSpawnMoreInCurrentWave = false;
                    StopSpawn();
                }

                yield return new WaitForSeconds(spawnedList[i].nextSpawnDelay);
            }
            yield return new WaitForSeconds(spawnedList[i].spawnNextGroupDelay);
        }
        StopSpawn();
    }

    public void Reset()
    {
        if (_spawnCoroutine != null)
            StopCoroutine(_spawnCoroutine);
        _currentWave = 0;
    }


    public void StopSpawn()
    {
        if (_spawnCoroutine != null)
        {
            StopCoroutine(_spawnCoroutine);
            _spawnCoroutine = null;
        }
    }

    public void OnOneSpanwableTerminated(PooledSpawnableProduct spawnedObject)
    {
        if(spawnedObject.GetComponent<Health>() != null)
            DamageableSpawnableCount -= 1;

        spawnedObject.onSpawnableObjectDestroyed -= OnOneSpanwableTerminated;
        int prevSize = _activeObjects.Count;
        _activeObjects.Remove(spawnedObject);
        Debug.Log($"SpawnManager.ActiveObject.Terminated: {prevSize} to {_activeObjects.Count}");

        if (!_canSpawnMoreInCurrentWave && _activeObjects.Count <= 0)
        {
            Debug.Log($"SpawnManager: Wave is finished!");
            onCurrentWaveFinished?.Invoke();
        }
    }

    public void OnDestroy()
    {
        foreach (var spawnedObject in _activeObjects)
            spawnedObject.onSpawnableObjectDestroyed -= OnOneSpanwableTerminated;
        _activeObjects.Clear();
        onCurrentWaveFinished.RemoveAllListeners();
        onDamageableSpawnableCountChanged = null;
    }
}
