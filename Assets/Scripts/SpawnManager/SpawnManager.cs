using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static WaveSO;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private WaveSO[] _waveSOList;
    [SerializeField] private int _currentWave;
    private Coroutine _spawnCoroutine;

    public int CurrentWave => _currentWave;
    
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
                Instantiate(spawnedList[i].prefab, PlaySceneGlobal.Instance.SpawnedObjectParent);
            }
            yield return new WaitForSeconds(spawnedList[i].nextSpawnDelay);
        }
    }

    public void StopSpawn()
    {
        if (_spawnCoroutine != null)
            StopCoroutine(_spawnCoroutine);
    }

    private void Start()
    {
        StartSpawn();
    }
}
