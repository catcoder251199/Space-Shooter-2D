using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class PowerupPopup : MonoBehaviour
{
    [SerializeField] private PowerupUIItem[] _powerupItemList;
    [SerializeField] private CanvasGroup _background;
    [SerializeField] private RectTransform _popup;

    public UnityEvent OnClickPowerup;

    public void Show(BuffSO[] powerupList)
    {
        gameObject.SetActive(true);
        for (int i = 0; i < _powerupItemList.Length; i++)
        {
            _powerupItemList[i].UpdateUI(powerupList[i]);
        }

        Sequence tweenSequence = DOTween.Sequence();
        _background.alpha = 0f;
        tweenSequence.Append(_background.DOFade(1f, 1f));
        _popup.localScale = Vector2.one * 0.2f;
        tweenSequence.Join(_popup.DOScale(Vector2.one * 1f, 1f).SetEase(Ease.OutBack));
    }

    public void OnClick(PowerupUIItem item)
    {
        Debug.Log("On Click Reward: " + item.powerupBuff.buffName);
        Player player = GameManager.Instance?.Player;
        if (player != null)
        {
            var buffable = player.GetComponent<BuffableEntity>();
            buffable.AddBuff(item.powerupBuff.Initialize(player.gameObject));
        }
        OnClickPowerup?.Invoke();
        Hide();
    }

    public void Hide()
    {
        Sequence tweenSequence = DOTween.Sequence();
        _background.alpha = 1f;
        tweenSequence.Append(_background.DOFade(0f, 1f));
        tweenSequence.Join(_popup.DOScale(Vector2.one * 0.2f, 1f).SetEase(Ease.InBack));
        tweenSequence.AppendCallback(() => { gameObject.SetActive(false); });
    }

}
