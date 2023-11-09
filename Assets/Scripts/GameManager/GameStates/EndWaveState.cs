using UnityEngine;
using TMPro;
using DG.Tweening;

namespace GameState
{
    [RequireComponent(typeof(GameManager))]
    public class EndWaveState : MonoBehaviour, IGameState
    {
        [SerializeField] TextMeshProUGUI _countDownText;
        private GameManager _gm;

        private void Awake()
        {
            _gm = GetComponent<GameManager>();
        }

        public void OnStateEnter() 
        {
            Debug.Log("Game End Wave State: Enter");
            _gm.SpawnManager.StopSpawn(); // * the spawn routine might be running. it must be stopped, otherwise the new spawn won't happen
            ShowClearText();
        }

        private void ShowClearText()
        {
            CanvasGroup canvasGroup = _countDownText.GetComponent<CanvasGroup>();
            float fromScale = 0.2f;
            float toScale = 1f;

            Sequence tweenSequence = DOTween.Sequence();

            _countDownText.gameObject.SetActive(true);
            _countDownText.rectTransform.localScale = Vector3.one * fromScale;
            canvasGroup.alpha = 1f;
            _countDownText.text = "CLEAR!";

            var scaleTween = _countDownText.rectTransform.DOScale(toScale, 1.5f).SetEase(Ease.OutQuad);
            var fadeOutTween = canvasGroup.DOFade(0f, 1f).SetDelay(0.5f);
            tweenSequence.Append(scaleTween);
            tweenSequence.Join(fadeOutTween);
            tweenSequence.AppendCallback(ShowRewardPopup);
        }

        private void ShowRewardPopup()
        {
            if (_gm.SpawnManager.HaveNextWave())
            {
                BuffSO[] _randList = _gm.PowerUpSpawner.GetRandRewardList();
                _gm.UIManager.PowerupPopup.Show(_randList);
            }
        }

        public void OnRewardPopupHidden()
        {
            Debug.Log("Spawn Next Wave");
            _countDownText.gameObject.SetActive(false);
            if (_gm.SpawnManager.HaveNextWave())
            {
                _gm.SpawnManager.MoveToNextWave();
                _gm.ChangeState(_gm.PlayState);
            }
            else // Should end the scene
            {
                _gm.EndState.Victory = true;
                _gm.ChangeState(_gm.EndState);
            }
        }

        public void OnStateExit() 
        {
            Debug.Log("Game End Wave State: Exit");
        }
        public void UpdateExecute() { }
        public void FixedUpdateExecute() { }
    } 
}