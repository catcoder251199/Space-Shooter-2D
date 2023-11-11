using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;

public class HomeManager : MonoBehaviour
{
    [SerializeField] TrackerSO _tracker;
    [SerializeField, Header("Space Ship")] Image _spaceShipImg;
    [SerializeField] TextMeshProUGUI _hpText;
    [SerializeField] TextMeshProUGUI _dmgText;
    [SerializeField] Image _rotateRing;
    [SerializeField] TwoButtonsPageController _spaceShipChangeController;
    [SerializeField] SpaceShipSO[] _spaceShipList;
    [SerializeField] int _currentSpaceshipIdx;
    [SerializeField] TwoButtonsPageController _levelChangeController;
    [SerializeField] TextMeshProUGUI _levelText;
    [SerializeField] TextMeshProUGUI _desText;

    [SerializeField, Header("Change Color Background")] Image _background;
    [SerializeField] Color _backgroundFromColor;
    [SerializeField] Color _backgroundToColor;
    [SerializeField, Header("Change Color Star")] CanvasGroup _stars;
    [SerializeField] float _starFadeFrom;
    [SerializeField] float _starFadeTo;
    [SerializeField, Header("Change Color Background")] TextMeshProUGUI _titleText;
    [SerializeField] Color _titleFromColor;
    [SerializeField] Color _titleToColor;


    public static void ChangeScene(int buildIdx)
    {
        SceneManager.LoadScene(buildIdx);
    }

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        // Get pre-stored info from scriptable object file
        _currentSpaceshipIdx = _tracker.selectedSpaceShip;
        UpdateCurrentSpaceShipPanel();
        UpdateSpaceShipController();
        UpdateCurrentLevelPanel();

        _rotateRing?.rectTransform.DORotate(new Vector3(0, 0, 360), 2f, RotateMode.FastBeyond360).SetLoops(-1).SetEase(Ease.Linear);

        // Change background color
        _background.color = _backgroundFromColor;
        var backgroundSequence = DOTween.Sequence();
        backgroundSequence.Append(_background.DOColor(_backgroundToColor, 2f));
        backgroundSequence.Append(_background.DOColor(_backgroundFromColor, 2f));
        backgroundSequence.SetLoops(-1);

        _stars.alpha = _starFadeFrom;
        var starSequence = DOTween.Sequence();
        starSequence.Append(_stars.DOFade(_starFadeTo, 2f));
        starSequence.Append(_stars.DOFade(_starFadeFrom, 2f));
        starSequence.SetLoops(-1);

        // Change background color
        _titleText.color = _titleFromColor;
        var titleSequence = DOTween.Sequence();
        titleSequence.Append(_titleText.DOColor(_titleToColor, 4f));
        titleSequence.Append(_titleText.DOColor(_titleFromColor, 4f));
        titleSequence.SetLoops(-1);

    }

    // Listeners

    public void OnSpaceShipChanged(int oldIdx, int newIdx)
    {
        ShowSpaceShip(newIdx);
    }

    public void OnLevelChanged(int oldLevel, int newLevel)
    {
        int lastLevel = _spaceShipList[_currentSpaceshipIdx].GetLastLevel();
        if (newLevel < 0 || newLevel > lastLevel)
            return;
        var info = _spaceShipList[_currentSpaceshipIdx];
        //save selection
        info.level = newLevel;
        // update level text
        UpdateCurrentLevelPanel();
        _hpText.text = info.GetCurrentHp().ToString();
        _dmgText.text = info.GetCurrentBaseDamage().ToString();
    }

    //--
    private void ShowSpaceShip(int idx)
    {
        if (idx < 0 || idx >= _spaceShipList.Length)
            return;

        _currentSpaceshipIdx = idx;
        UpdateCurrentSpaceShipPanel();
        UpdateCurrentLevelPanel();

        // save selection
        _tracker.selectedSpaceShip = idx;
    }

    private void UpdateCurrentSpaceShipPanel()
    {
        var info = _spaceShipList[_currentSpaceshipIdx];
        _spaceShipImg.sprite = info.spaceShipSprite;
        _hpText.text = info.GetCurrentHp().ToString();
        _dmgText.text = info.GetCurrentBaseDamage().ToString();
        _desText.text = info.description;
    }

    private void UpdateSpaceShipController()
    {
        _spaceShipChangeController.SetMaxPageIdx(Mathf.Max(0, _spaceShipList.Length - 1), false);
        _spaceShipChangeController.SetCurrentPageIdx(_currentSpaceshipIdx, false);
        _spaceShipChangeController.UpdateAllButtons();
    }

    private void UpdateCurrentLevelPanel()
    {
        int currentLevel = _spaceShipList[_currentSpaceshipIdx].GetCurrentLevel();
        _levelText.text = "Level " + currentLevel.ToString();
        _levelChangeController.SetMaxPageIdx(_spaceShipList[_currentSpaceshipIdx].GetLastLevel(), false);
        _levelChangeController.SetCurrentPageIdx(currentLevel, false);
        _levelChangeController.UpdateAllButtons();

    }

    public void PlayGame(int sceneIdx)
    {
        _tracker.selectedSpaceShipSO = _spaceShipList[_currentSpaceshipIdx];
        SceneManager.LoadScene(sceneIdx);
    }

    public void OnDisable()
    {
        DOTween.KillAll();
    }
}
