using UnityEngine;
using GameState;
using DG.Tweening;
using TMPro.EditorUtilities;

public class GameManager : MonoBehaviour
{
    UIManager _uiManager;

    public static GameManager Instance;

    private SpawnManager _spawnManager;
    private PowerUpSpawner _powerupSpawner;
    private Player _player;
    private Camera _cam;

    private IGameState _currentState;
    private StartState _startState;
    private PlayState _playState;
    private EndWaveState _endWaveState;
    private EndState _endState;

    public Player Player => _player;
    public Camera Camera => _cam;
    public StartState StartState => _startState;
    public PlayState PlayState => _playState;
    public EndWaveState EndWaveState => _endWaveState;
    public EndState EndState => _endState;
    public SpawnManager SpawnManager => _spawnManager;
    public UIManager UIManager => _uiManager;
    public PowerUpSpawner PowerUpSpawner => _powerupSpawner;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            Init();
        }
    }

    public bool Paused { get; set; } = false;

    private void Init()
    {
        _cam = Camera.main;
        _player = FindPlayer();
        _spawnManager = FindSpawnManager();
        _powerupSpawner = FindPowerupSpawner();
        _uiManager = GetComponent<UIManager>();

        _startState = GetComponent<StartState>();
        _playState = GetComponent<PlayState>();
        _endWaveState = GetComponent<EndWaveState>();
        _endState = GetComponent<EndState>();
    }

    private void OnEnable()
    {
        _player.OnPlayerDiedEvent.AddListener(OnPlayerDied);
    }

    private void OnDisable()
    {
        _player.OnPlayerDiedEvent.RemoveListener(OnPlayerDied);
    }

    private Player FindPlayer()
    {
        Player player = GameObject.FindGameObjectWithTag("Player")?.GetComponent<Player>();
        if (player == null)
            player = FindObjectOfType<Player>();
        return player;
    }

    private SpawnManager FindSpawnManager()
    {
        SpawnManager spawnManager = GameObject.FindGameObjectWithTag("SpawnManager")?.GetComponent<SpawnManager>();
        if (spawnManager == null)
            spawnManager = FindObjectOfType<SpawnManager>();
        return spawnManager;
    }
    private PowerUpSpawner FindPowerupSpawner()
    {
        PowerUpSpawner powerupSpawner = GameObject.FindGameObjectWithTag("SpawnManager")?.GetComponent<PowerUpSpawner>();
        if (powerupSpawner == null)
            powerupSpawner = FindObjectOfType<PowerUpSpawner>();
        return powerupSpawner;
    }

    public virtual void ChangeState(IGameState _nextState)
    {
        if (_nextState == null)
            return;

        _currentState?.OnStateExit();
        _currentState = _nextState;
        _currentState?.OnStateEnter();
    }

    public void Start()
    {
        ChangeState(_startState);
    }

    private void OnPlayerDied()
    {
        _endState.Victory = false;
        ChangeState(_endState);
    }

    //--Controlling Utilized Methods
    public void SetCameraFollowActive(bool enabled)
    {
        CameraFollowTarget followTarget = Camera.GetComponent<CameraFollowTarget>();
        if (followTarget != null)
            followTarget.enabled = enabled;
    }

    public void SetPlayerControllerActive(bool enabled)
    {
        PlayerController controller = Player?.GetComponent<PlayerController>();
        if (controller != null)
            controller.enabled = enabled;
    }
}
