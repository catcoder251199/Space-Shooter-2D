using UnityEngine;
using DG.Tweening;
using TMPro;
using System.Collections.Generic;
using System.Collections;
using Unity.VisualScripting;
using TMPro.EditorUtilities;

[RequireComponent(typeof(GameManager))]
public class UIManager : MonoBehaviour
{
    [SerializeField, Header("Boss Health Bar")] RectTransform _bossHealthBar;
    private HealthBar _bossHpBar;
    //--
    [SerializeField, Header("Buff Panel")] private RectTransform _buffDescrPanel;
    [SerializeField] private TextMeshProUGUI _buffDescrText;
    private Queue<BuffSO> _buffs = new Queue<BuffSO>();
    private Coroutine _showBuffRoutine;
    //--
    [SerializeField] private PowerupPopup _powerupPopup;
    public PowerupPopup PowerupPopup => _powerupPopup;
    //--
    [SerializeField] private VictoryPopup _victoryPopup;
    public VictoryPopup VictoryPopup => _victoryPopup;
    private void Awake()
    {
        _bossHpBar = _bossHealthBar.GetComponent<HealthBar>();
    }

    public void ShowBuffDescriptionPanel(BuffSO buff)
    {
        _buffs.Enqueue(buff);
        if (_showBuffRoutine == null)
           _showBuffRoutine = StartCoroutine(ShowBuffRoutine());
    }

    private IEnumerator ShowBuffRoutine()
    {
        _buffDescrPanel.gameObject.SetActive(true);
        _buffDescrText.text = "";
        while (_buffs.Count > 0)
        {
            var buff = _buffs.Dequeue();
            _buffDescrText.text = buff.description;
            _buffDescrPanel.localScale = Vector3.one * 0.8f;
            var tween = DOTween.Sequence();
            tween.Append(_buffDescrPanel.DOScale(1f, 0.5f).SetEase(Ease.OutQuad));
            tween.Append(_buffDescrPanel.DOScale(0.5f, 0.5f).SetDelay(1f).SetEase(Ease.InQuad));
            yield return tween.WaitForCompletion();
        }
        _buffDescrPanel.gameObject.SetActive(false);
        _showBuffRoutine = null;
    }


    public void ShowBossHealth()
    {
        _bossHealthBar.gameObject.SetActive(true);
        _bossHealthBar.anchoredPosition = new Vector2(0, 60);
        _bossHealthBar.DOAnchorPosY(-30, 1f);
    }

    public void UpdateBossHealth(int value, int max)
    {
        _bossHpBar.SetHpMax(max);
        _bossHpBar.SetHpValue(value);
    }

    public void HideBossHealth()
    {
        _bossHealthBar.DOAnchorPosY(60, 1f)
            .OnComplete(() => { _bossHealthBar.gameObject.SetActive(false); });
    }
}