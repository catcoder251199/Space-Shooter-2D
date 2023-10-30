using System.Collections;
using System.Collections.Generic;
using UnityEngine.Pool;
using UnityEngine;
using static WaveSO;
using UnityEngine.Events;

public class SpawnManager : MonoBehaviour
{
    public UnityEvent onCurrentWaveFinished;
    public UnityEvent onAllWavesFinished;

    [SerializeField] private WaveSO[] _waveSOList;
    [SerializeField] private int _currentWave;

    private Coroutine _spawnCoroutine;
    public int CurrentWave => _currentWave;

    [SerializeField] SpawnableFactory _factory;
    private HashSet<PooledSpawnableProduct> _activeObjects = new HashSet<PooledSpawnableProduct>();

    private void Awake()
    {
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

    private IEnumerator SpawnRoutine()
    {
        WaveSO waveSO = _waveSOList[_currentWave];
        SpawnedObject[] spawnedList = waveSO.SpawnedObjectsList;
        int spawnedCount = spawnedList.Length;
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

                spawnableObject.onSpawnableObjectReleased += OnOneSpanwableTerminated;
                spawnableObject.onSpawnableObjectDestroyed += OnOneSpanwableTerminated;
                yield return new WaitForSeconds(spawnedList[i].nextSpawnDelay);
            }
            yield return new WaitForSeconds(spawnedList[i].spawnNextGroupDelay);
        }
        StopSpawn();
        Debug.Log("On Wave finished");
    }

    public void SpawnNext()
    {
        if (_currentWave + 1 < _waveSOList.Length)
        {
            _currentWave++;
            StartSpawn();
        }
        else
        {
            onCurrentWaveFinished.RemoveListener(SpawnNext);
            onAllWavesFinished?.Invoke(); // win the game
        }
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
        spawnedObject.onSpawnableObjectReleased -= OnOneSpanwableTerminated;
        int prevSize = _activeObjects.Count;
        _activeObjects.Remove(spawnedObject);
        Debug.Log($"SpawnManager.ActiveObject.Terminated: {prevSize} to {_activeObjects.Count}");

        if (_activeObjects.Count <= 0)
        {
            Debug.Log($"SpawnManager: Wave is finished!");
            onCurrentWaveFinished?.Invoke();
        }
    }
    private void Start()
    {
        StartSpawn();
    }
}
