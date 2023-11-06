using UnityEngine;
using TMPro;
using DG.Tweening;

namespace GameState
{
    [RequireComponent(typeof(GameManager))]
    public class PlayState : MonoBehaviour, IGameState
    {
        [SerializeField] TextMeshProUGUI _waveStartText;
        [SerializeField] TextMeshProUGUI _turnCountText;
        [SerializeField] RectTransform _waveStartPanel;

        private GameManager _gm;

        private void Awake()
        {
            _gm = GetComponent<GameManager>();
        }

        private void Start()
        {
            _gm.SpawnManager.onCurrentWaveFinished.AddListener(OnCurrentWaveFinished);
            _turnCountText.text = $"{_gm.SpawnManager.CurrentWave + 1}/{_gm.SpawnManager.wavesTotal}";
        }

        private void OnDisable()
        {
            _gm.SpawnManager.onCurrentWaveFinished.RemoveListener(OnCurrentWaveFinished);
        }

        private void OnCurrentWaveFinished()
        {
            _gm.ChangeState(_gm.EndWaveState);
        }

        public void OnStateEnter() 
        {
            Debug.Log("Game Play State: Enter");
            ShowWaveTurnText();
        }

        public void ShowWaveTurnText()
        {
            if (_waveStartText != null)
                _waveStartText.text = "Wave " + (_gm.SpawnManager.CurrentWave + 1);
            _turnCountText.text = $"{_gm.SpawnManager.CurrentWave + 1}/{_gm.SpawnManager.wavesTotal}";

            if (_waveStartPanel != null)
            {
                _waveStartPanel.gameObject.SetActive(true);
                _waveStartPanel.sizeDelta = new Vector2(0, 120);
                var tweenSequence = DOTween.Sequence()
                    .Append(_waveStartPanel.DOSizeDelta(new Vector2(350, 120), 1f))
                    .AppendInterval(1f)
                    .Append(_waveStartPanel.DOSizeDelta(new Vector2(350, 0), 1f))
                    .AppendCallback(StartTheWave);
            }
        }

        public void StartTheWave()
        {
            _waveStartPanel.gameObject.SetActive(false);
            _gm.SetPlayerControllerActive(true);
            _gm.SpawnManager.StartSpawn();
        }

        public void OnStateExit() 
        {
            Debug.Log("Game Play State: Exit");
        }
        public void UpdateExecute() { }
        public void FixedUpdateExecute() { }
    } 
}