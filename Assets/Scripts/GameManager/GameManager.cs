using UnityEngine;
using GameState;
using DG.Tweening;
using TMPro.EditorUtilities;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    UIManager _uiManager;

    [SerializeField] Player _playerPrefab;
    [SerializeField] Transform _playerStartTransform;
    [SerializeField] TrackerSO _tracker;

    public static GameManager Instance;
    public static bool gameIsPaused;

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

        ContinueGame();
    }

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

    public void Start()
    {
        ChangeState(_startState);
    }
    public virtual void ChangeState(IGameState _nextState)
    {
        if (_nextState == null)
            return;

        _currentState?.OnStateExit();
        _currentState = _nextState;
        _currentState?.OnStateEnter();
    }

    private void Update()
    {
        if (gameIsPaused)
            return;

        _currentState.UpdateExecute();
    }
    
    private void FixedUpdate()
    {
        if (gameIsPaused)
            return;

        _currentState.FixedUpdateExecute();
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
        Player player = Instantiate(_playerPrefab, _playerStartTransform.position, Quaternion.identity);
        var spaceshipInfo = _tracker.selectedSpaceShipSO;
        if (spaceshipInfo != null)
            player.Init(spaceshipInfo);
        else
            player.Init();

        CameraFollowTarget followComponent = Camera.main.GetComponent<CameraFollowTarget>();
        followComponent.SetTarget(player?.GetComponent<Rigidbody2D>());

        return player;

        //Player player = GameObject.FindGameObjectWithTag("Player")?.GetComponent<Player>();
        //if (player == null)
        //    player = FindObjectOfType<Player>();
        //return player;
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

    public static void PauseGame()
    {
        gameIsPaused = true;
        Time.timeScale = 0;
    }

    public static void ContinueGame()
    {
        gameIsPaused = false;
        Time.timeScale = 1;
    }

    public static void RestartScene()
    {
        DOTween.KillAll();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public static void ChangeToScene(int buildIdx)
    {
        DOTween.KillAll();
        SceneManager.LoadScene(buildIdx);
    }
}
