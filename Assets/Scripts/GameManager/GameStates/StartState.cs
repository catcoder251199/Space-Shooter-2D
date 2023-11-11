using UnityEngine;
using TMPro;
using DG.Tweening;
using System;

namespace GameState
{
    [RequireComponent(typeof(GameManager))]
    public class StartState : MonoBehaviour, IGameState
    {
        [SerializeField] private RectTransform _leftHUDTopPanel;
        [SerializeField] private RectTransform _rightHUDTopPanel;
        [SerializeField] private TextMeshProUGUI _countDownText;

        private GameManager _gm;

        private void Awake()
        {
            _gm = GetComponent<GameManager>();
        }

        public void OnStateEnter() 
        {
            Debug.Log("Game Start State: Enter");
            _leftHUDTopPanel.DOAnchorPos(new Vector2(-120, _leftHUDTopPanel.anchoredPosition.y), 1f).From();
            _rightHUDTopPanel.DOAnchorPos(new Vector2(120, _rightHUDTopPanel.anchoredPosition.y), 1f).From();
            MovePlayerToStartPosition();
        }

        private void MovePlayerToStartPosition()
        {
            Player player = _gm.Player;
            if (player == null) return;
            Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
            _gm.SetCameraFollowActive(false);
            _gm.SetPlayerControllerActive(false);
            playerRb.DOMoveY(-12, 1f).From().OnComplete(CountDownToStartGame);
        }

        private void CountDownToStartGame()
        {
            _countDownText.gameObject.SetActive(true);
            _countDownText.text = "";
            float scaleTo = 1.5f;
            float scaleDuration = 1f;

            var tweenSequence = DOTween.Sequence();
            Action<string> onFinishedScaleCallback =  (string text) => {
                _countDownText.text = text;
                _countDownText.rectTransform.localScale = Vector3.one * 0.5f;
            };
            tweenSequence.AppendInterval(1f);
            tweenSequence.AppendCallback(() => onFinishedScaleCallback("3"));
            tweenSequence.Append(_countDownText.rectTransform.DOScale(scaleTo, scaleDuration).SetEase(Ease.OutBack));
            tweenSequence.AppendCallback(() => onFinishedScaleCallback("2"));
            tweenSequence.Append(_countDownText.rectTransform.DOScale(scaleTo, scaleDuration).SetEase(Ease.OutBack));
            tweenSequence.AppendCallback(() => onFinishedScaleCallback("1"));
            tweenSequence.Append(_countDownText.rectTransform.DOScale(scaleTo, scaleDuration).SetEase(Ease.OutBack));
            tweenSequence.AppendCallback(() => onFinishedScaleCallback("GO"));
            tweenSequence.Append(
                _countDownText.rectTransform.DOScale(scaleTo, scaleDuration)
                .SetEase(Ease.OutBack)
                .OnComplete(() =>
                {
                    _countDownText.gameObject.SetActive(false);
                    _countDownText.text = "";
                    OnCountDownCompleted();
                }));
        }

        void OnCountDownCompleted()
        {
            _gm.SetCameraFollowActive(true);
            _gm.PowerUpSpawner.StartSpawn();
            _gm.ChangeState(_gm.PlayState);
        }

        public void OnStateExit() 
        {
            Debug.Log("Game Start State: Exit");
        }
        public void UpdateExecute() { }
        public void FixedUpdateExecute() { }
    } 
}