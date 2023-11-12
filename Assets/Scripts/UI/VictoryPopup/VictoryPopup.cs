using DG.Tweening;
using TMPro;
using UnityEngine;

public class VictoryPopup : MonoBehaviour
{
    [SerializeField] private CanvasGroup _background;
    [SerializeField] private ParticleSystem _confettiEffect;
    [SerializeField] private RectTransform _confettiTransform;
    [SerializeField] private TextMeshProUGUI _victoryText;
    [SerializeField] private TypeMessage _typeMessage;
    [SerializeField, TextArea] private string[] _victoryMsgList;
    [SerializeField, TextArea] private string[] _defeatMsgList;

    [SerializeField, Header("Sound")] private AudioClip _victorySound;
    [SerializeField] private AudioClip _defeatSound;
    public void ShowVictory()
    {
        gameObject.SetActive(true);
        var tweenSequence = DOTween.Sequence();
        _background.alpha = 0;
        tweenSequence.Append(_background.DOFade(1f, 1f));

        _victoryText.text = "VICTORY";
        if (_victorySound != null)
            tweenSequence.AppendCallback(() => { SoundManager.Instance.PlayEffectOneShot(_victorySound); });
        tweenSequence.Append(_victoryText.rectTransform.DOScale(0.5f, 1f).From().SetEase(Ease.InOutBack));
        tweenSequence.AppendCallback(() =>
        {
            if (_confettiEffect != null)
                Instantiate(_confettiEffect, _confettiTransform.position, _confettiEffect.transform.rotation);
            
            _typeMessage.SetMessageList(_victoryMsgList);
            _typeMessage.StartType();
        });
    }
    public void ShowDefeat()
    {
        gameObject.SetActive(true);
        var tweenSequence = DOTween.Sequence();
        _background.alpha = 0;
        tweenSequence.Append(_background.DOFade(1f, 1f));

        _victoryText.text = "DEFEAT";
        if (_defeatSound != null)
            tweenSequence.AppendCallback(() => { SoundManager.Instance.PlayEffectOneShot(_defeatSound); });
        tweenSequence.Append(_victoryText.rectTransform.DOAnchorPos(new Vector3(0, 100, 0), 1f).From().SetEase(Ease.OutBounce));
        tweenSequence.AppendCallback(() => {
            string[] msgChosen = { _defeatMsgList[Random.Range(0, _defeatMsgList.Length)] };
            _typeMessage.SetMessageList(msgChosen);
            _typeMessage.StartType();
        });
    }

    public void Hide()
    {
        Sequence tweenSequence = DOTween.Sequence();
        _background.alpha = 1f;
        tweenSequence.Append(_background.DOFade(0f, 1f));
        tweenSequence.AppendCallback(() => { gameObject.SetActive(false); });
    }
}
