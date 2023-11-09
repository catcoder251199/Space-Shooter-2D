using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    [SerializeField] private SpawnableFactory _patternFactory;
    [SerializeField, Header("Power-ups")] private SpawnableFactory _powerupFactory;
    [SerializeField, Tooltip("Number of spawned power-ups per minute"), Range(0, 60)] private int _powerupSpawnRate = 1;
    [SerializeField] private int _maxPowerUpCountPerSpawn = 1;

    [SerializeField, Header("Heal Up")] private PowerUp _healUpPrefab;
    [SerializeField, Tooltip("Number of spawned healing-up power-ups per minute"), Range(0, 60)] private int _healSpawnRate = 1;
    [SerializeField] private int _maxHealUpCountPerSpawn = 1;

    [SerializeField, Header("others")] private float _powerupMinYSpeed = 1f;
    [SerializeField] private float _powerupMaxYSpeed = 1f;
    [SerializeField] private float _maxPowerupDelay = 1f;
    [SerializeField] private float _powerupScale = 1f;


    private Coroutine _spawnRoutine;
    private void Awake()
    {
        _maxPowerUpCountPerSpawn = Mathf.Max(1, _maxPowerUpCountPerSpawn);
    }

    public void StartSpawn()
    {
       if (_spawnRoutine == null)
            _spawnRoutine = StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        // Every second, Check whether we should spawn a new powerup based on spawn probability
        while (true)
        {
            bool doSpawn = Random.Range(0f, 1f) <= _powerupSpawnRate / 60;
            if (doSpawn)
                yield return StartCoroutine(SpawnRandPowerup(Random.Range(1, _maxPowerUpCountPerSpawn + 1)));

            bool doSpawnHeal = Random.Range(0f, 1f) <= _healSpawnRate / 60;
            if (doSpawnHeal)
                yield return StartCoroutine(SpawnHealUp(Random.Range(1, _maxHealUpCountPerSpawn + 1)));

            yield return new WaitForSeconds(1);
        }
    }
    private IEnumerator SpawnRandPowerup(int count)
    {
        for (int i = 0; i < count; i++)
        {
            int instanceId = _powerupFactory.GetRandomInstanceIdInPool();
            PowerUp powerUp = _powerupFactory.CreateSpawnableProduct(instanceId)?.GetComponent<PowerUp>();
            if (powerUp == null)
                Debug.LogError("PowerUpSpawner.SpawnRandPowerup: Get a null powerup");
            else
            {
                powerUp.transform.position = Helper.Cam.GetTopSideRandomPosRange(2, 0, 0.2f, 0.8f);
                powerUp.transform.localScale = Vector2.one * _powerupScale;
                powerUp.moveVec = new Vector2(0, Random.Range(-_powerupMinYSpeed, -_powerupMaxYSpeed));
                powerUp.rotateSpeed = 0;
            }
            yield return new WaitForSeconds(Random.Range(0f, _maxPowerupDelay));
        }
    }
    public void SpawnRandShootPatterns(int count)
    {
       StartCoroutine(SpawnRandPatternRoutine(count));
    }

    private IEnumerator SpawnRandPatternRoutine(int count)
    {
        for (int i = 0; i < count; ++i)
        {
            int instanceId = _patternFactory.GetRandomInstanceIdInPool();
            PowerUp powerUp = _powerupFactory.CreateSpawnableProduct(instanceId)?.GetComponent<PowerUp>();
            if (powerUp == null)
                Debug.LogError("PowerUpSpawner.SpawnRandShootPatterns: Get a null powerup");
            else
            {
                powerUp.transform.position = Helper.Cam.GetTopSideRandomPosRange(2, 0, 0.2f, 0.8f);
                powerUp.transform.localScale = Vector2.one * _powerupScale;
                powerUp.moveVec = new Vector2(0, Random.Range(-_powerupMinYSpeed, -_powerupMaxYSpeed));
                powerUp.rotateSpeed = 0;
            }
            yield return new WaitForSeconds(Random.Range(0f, _maxPowerupDelay));
        }
    }

    public IEnumerator SpawnHealUp(int count)
    {
        for (int i = 0; i < count; ++i)
        {
            int healUpId = _healUpPrefab.gameObject.GetInstanceID();
            PowerUp powerUp = _powerupFactory.CreateSpawnableProduct(healUpId)?.GetComponent<PowerUp>();
            if (powerUp == null)
                Debug.LogError("PowerUpSpawner.SpawnRandPowerup: Get a null HealUp");
            else
            {
                powerUp.transform.position = Helper.Cam.GetTopSideRandomPosRange(2, 0, 0.2f, 0.8f);
                powerUp.transform.localScale = Vector2.one * _powerupScale;
                powerUp.moveVec = new Vector2(0, Random.Range(-_powerupMinYSpeed, -_powerupMaxYSpeed));
                powerUp.rotateSpeed = 0;
            }
            yield return new WaitForSeconds(Random.Range(0f, _maxPowerupDelay));
        }
    }

    public void StopSpawn()
    {
        if (_spawnRoutine != null)
        {
            StopCoroutine(_spawnRoutine);
            _spawnRoutine = null;
        }
    }
}
