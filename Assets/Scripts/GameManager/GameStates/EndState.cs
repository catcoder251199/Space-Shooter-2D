using UnityEngine;
using TMPro;
using DG.Tweening;

namespace GameState
{
    [RequireComponent(typeof(GameManager))]
    public class EndState : MonoBehaviour, IGameState
    {
        private GameManager _gm;
        [SerializeField] private RectTransform _endPanel;
        [SerializeField] private TextMeshProUGUI _victoryText;

        public bool Victory { get; set; }

        private void Awake()
        {
            _gm = GetComponent<GameManager>();
        }

        public void OnStateEnter() 
        {
            Debug.Log("Game End State: Enter");
            _gm.Player.Controller.enabled = false;
            _gm.Player.WeaponHandler.Deactivate();
            _gm.SpawnManager.StopSpawn();
            _gm.PowerUpSpawner.StopSpawn();
            ShowEndPanel();
        }

        private void ShowEndPanel()
        {
            if (Victory)
                ShowVictoryEndPanel();
            else
                ShowDefeatEndPanel();
        }

        private void ShowVictoryEndPanel()
        {
            _endPanel.gameObject.SetActive(true);
            Sequence sequence = DOTween.Sequence();

            CanvasGroup canvasGroup = _endPanel.GetComponent<CanvasGroup>();
            canvasGroup.alpha = 0f;
            sequence.Append(canvasGroup.DOFade(1f, 0.75f));

            _victoryText.text = "VICTORY";
            sequence.Append(_victoryText.rectTransform.DOScale(0.2f, 1f).From().SetEase(Ease.InOutBack));
        }

        private void ShowDefeatEndPanel()
        {
            _endPanel.gameObject.SetActive(true);
            Sequence sequence = DOTween.Sequence();

            CanvasGroup canvasGroup = _endPanel.GetComponent<CanvasGroup>();
            canvasGroup.alpha = 0f;
            sequence.Append(canvasGroup.DOFade(1f, 0.75f));

            _victoryText.text = "DEFEAT";
            sequence.Append(_victoryText.rectTransform.DOAnchorPos(new Vector3(0, 150, 0), 1f).From().SetEase(Ease.OutBounce));
        }

        public void OnStateExit() 
        {
            Debug.Log("Game End State: Exit");
        }
        public void UpdateExecute() { }
        public void FixedUpdateExecute() { }
    } 
}