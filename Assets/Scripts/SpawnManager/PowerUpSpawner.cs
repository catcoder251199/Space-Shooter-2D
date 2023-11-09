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

    [SerializeField, Header("Rewards")] private BuffSO[] _rewardList;

    [SerializeField, Header("others")] private float _powerupMinYSpeed = 1f;
    [SerializeField] private float _powerupMaxYSpeed = 1f;
    [SerializeField] private float _maxPowerupDelay = 1f;
    [SerializeField] private float _powerupScale = 1f;

    private Coroutine _mainSpawnRoutine;
    private void Awake()
    {
        _maxPowerUpCountPerSpawn = Mathf.Max(1, _maxPowerUpCountPerSpawn);
    }

    public void StartSpawn()
    {
       if (_mainSpawnRoutine == null)
            _mainSpawnRoutine = StartCoroutine(SpawnMainRoutine());
    }

    public void StopSpawn()
    {
        if (_mainSpawnRoutine != null)
        {
            StopCoroutine(_mainSpawnRoutine);
            _mainSpawnRoutine = null;
        }
    }

    private IEnumerator SpawnMainRoutine()
    {
        // Every second, Check whether we should spawn a new powerup based on spawn probability
        while (true)
        {
            bool doSpawn = Random.Range(0f, 1f) <= _powerupSpawnRate / 60;
            if (doSpawn)
                yield return StartCoroutine(SpawnRandPowerupRoutine(Random.Range(1, _maxPowerUpCountPerSpawn + 1)));

            bool doSpawnHeal = Random.Range(0f, 1f) <= _healSpawnRate / 60;
            if (doSpawnHeal)
                yield return StartCoroutine(SpawnHealUpRoutine(Random.Range(1, _maxHealUpCountPerSpawn + 1)));

            yield return new WaitForSeconds(1);
        }
    }
    // ---
    private IEnumerator SpawnHealUpRoutine(int count)
    {
        for (int i = 0; i < count; ++i)
        {
            int healUpId = _healUpPrefab.gameObject.GetInstanceID();
            PowerUp powerUp = _powerupFactory.CreateSpawnableProduct(healUpId)?.GetComponent<PowerUp>();
            if (powerUp == null)
                Debug.LogError("PowerUpSpawner.SpawnHealUpRoutine: Get a null HealUp");
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
    private IEnumerator SpawnRandPowerupRoutine(int count)
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
    private IEnumerator SpawnRandPatternRoutine(int count)
    {
        for (int i = 0; i < count; ++i)
        {
            int instanceId = _patternFactory.GetRandomInstanceIdInPool();
            PowerUp powerUp = _patternFactory.CreateSpawnableProduct(instanceId)?.GetComponent<PowerUp>();
            if (powerUp == null)
                Debug.LogError("PowerUpSpawner.SpawnRandPatternRoutine: Get a null powerup");
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
    public void SpawnRandPowerups(int count)
    {
        StartCoroutine(SpawnRandPowerupRoutine(count));
    }

    public BuffSO[] GetRandRewardList()
    {
        BuffSO[] ret = new BuffSO[3];
        for (int i = 0; i < 3; i++)
            ret[i] = _rewardList[Random.Range(0, _rewardList.Length)];
        return ret;
    }
}
