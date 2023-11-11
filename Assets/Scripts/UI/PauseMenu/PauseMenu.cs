using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private CanvasGroup _background;
    [SerializeField] private RectTransform _pausePanel;
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _soundEffectSlider;
    [SerializeField] private ToggleButton _musicToggleBtn;
    [SerializeField] private ToggleButton _effectToggleBtn;

    private void Start()
    {
        _musicSlider.SetValueWithoutNotify(SoundManager.Instance.GetMusicVolume());
        _soundEffectSlider.SetValueWithoutNotify(SoundManager.Instance.GetEffectVolume());
        _musicToggleBtn.SetToggleWithoutNotify(SoundManager.Instance.GetMusicVolume() > 0);
        _effectToggleBtn.SetToggleWithoutNotify(SoundManager.Instance.GetEffectVolume() > 0);
    }

    public void Show()
    {
        gameObject.SetActive(true);
        var tweenSequence = DOTween.Sequence();
        _background.alpha = 0;
        tweenSequence.Append(_background.DOFade(1f, 1f));
        _pausePanel.localScale = Vector3.one * 0.2f;
        tweenSequence.Join(_pausePanel.DOScale(1f, 1f).SetEase(Ease.OutBack));
        tweenSequence.AppendCallback(() =>
        {
            PauseGame();
        });
    }
    public void PauseGame()
    {
        GameManager.PauseGame();
    }

    public void ContinueGame()
    {
        GameManager.ContinueGame();
    }

    public void Hide()
    {
        ContinueGame();

        Sequence tweenSequence = DOTween.Sequence();
        _background.alpha = 1f;
        tweenSequence.Append(_background.DOFade(0f, 1f));
        tweenSequence.Join(_pausePanel.DOScale(Vector2.one * 0.2f, 1f).SetEase(Ease.InBack));
        tweenSequence.AppendCallback(() => { gameObject.SetActive(false); });
    }
    public void OnToggleMusic(bool on)
    {
        float value = on ? 1 : 0;
        _musicSlider.SetValueWithoutNotify(value);
        SoundManager.Instance.ChangeMusicSourceVolume(value);
    }

    public void OnToggleSoundEffect(bool on)
    {
        float value = on ? 1 : 0;
        _soundEffectSlider.SetValueWithoutNotify(value);
        SoundManager.Instance.ChangeEffectVolume(value);
    }

    public void OnMusicSliderChanged(float value)
    {
        if (Mathf.Approximately(value, 0))
            _musicToggleBtn.SetToggleWithoutNotify(false);
        else
            _musicToggleBtn.SetToggleWithoutNotify(true);
        SoundManager.Instance.ChangeMusicSourceVolume(value);
    }

    public void OnSoundEffectSliderChanged(float value)
    {
        if (Mathf.Approximately(value, 0))
            _musicToggleBtn.SetToggleWithoutNotify(false);
        else
            _musicToggleBtn.SetToggleWithoutNotify(true);
        SoundManager.Instance.ChangeEffectVolume(value);
    }
}
