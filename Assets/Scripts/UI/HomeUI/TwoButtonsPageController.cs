using TMPro;
using UnityEngine;

public class TwoButtonsPageController : MonoBehaviour
{
    public IntIntEvent onPageIdxChanged; // from: int, to: int
    [SerializeField] RectTransform _leftButton;
    [SerializeField] RectTransform _rightButton;

    [SerializeField] private int _currentIdx = 0;
    [SerializeField] private int _maxPageIdx = 0;

    [SerializeField] private bool _soundOnClick = true;
    
    public int GetMaxPageIdx()
    {
        return _maxPageIdx;
    }

    public void SetMaxPageIdx(int maxPageIdx, bool update = true)
    {
        _maxPageIdx = Mathf.Max(maxPageIdx, 0);
        //if (maxPageIdx == _maxPageIdx) return;
        _currentIdx = Mathf.Clamp(_currentIdx, 0, _maxPageIdx);
        if (update)
            UpdateAllButtons();
    }

    public int GetCurrentPageIdx()
    {
        return _currentIdx;
    }

    public void SetCurrentPageIdx(int newIdx, bool update = true)
    {
        if (newIdx < 0 || newIdx > _maxPageIdx) return; // out of range
        //if (newIdx == _currentIdx) return; // same as current index

        _currentIdx = newIdx;
        _currentIdx = Mathf.Clamp(_currentIdx, 0, _maxPageIdx);
        if (update)
            UpdateAllButtons();
    }

    public void UpdateAllButtons()
    {
        if (_currentIdx == 0)
            _leftButton.gameObject.SetActive(false);

        if (_currentIdx == _maxPageIdx)
            _rightButton.gameObject.SetActive(false);


        if (!_rightButton.gameObject.activeSelf)
            if (_currentIdx < _maxPageIdx)
                _rightButton.gameObject.SetActive(true);

        if (!_leftButton.gameObject.activeSelf)
            if (_currentIdx > 0)
                _leftButton.gameObject.SetActive(true);
    }

    public void OnClickLeft()
    {
        int oldIdx = _currentIdx;
        _currentIdx = Mathf.Max(_currentIdx - 1, 0);

        if (_currentIdx == 0)
            _leftButton.gameObject.SetActive(false);

        if(!_rightButton.gameObject.activeSelf)
            if (_currentIdx < _maxPageIdx)
                _rightButton.gameObject.SetActive(true);

        if (_soundOnClick)
            SoundManager.Instance.playButtonClickSound();

        OnChangePageIdx(oldIdx, _currentIdx);
    }

    public void OnClickRight()
    {
        int oldIdx = _currentIdx;
        _currentIdx = Mathf.Min(_currentIdx + 1, _maxPageIdx);

        if (_currentIdx == _maxPageIdx)
            _rightButton.gameObject.SetActive(false);

        if (!_leftButton.gameObject.activeSelf)
            if (_currentIdx > 0)
                _leftButton.gameObject.SetActive(true);

        if (_soundOnClick)
            SoundManager.Instance.playButtonClickSound();

        OnChangePageIdx(oldIdx, _currentIdx);
    }

    public void OnChangePageIdx(int from, int to)
    {
        onPageIdxChanged?.Invoke(from, to);
    }
}