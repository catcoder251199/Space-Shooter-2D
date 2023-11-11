using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ToggleButton : MonoBehaviour
{
    [SerializeField] private Sprite _onSprite;
    [SerializeField] private Sprite _offSprite;
    [SerializeField] private bool _on;
    [SerializeField] private Image _image;

    public BoolEvent onToggledEvent;

    private void Awake()
    {
        SetToggleWithoutNotify(_on);
    }

    public void OnToggled()
    {
        _on = !_on;
        if (_image != null)
            _image.sprite = _on ? _onSprite : _offSprite;
        onToggledEvent?.Invoke(_on);
    }

    public void SetToggleWithoutNotify(bool on)
    {
        _on = on;
        if (_image != null)
            _image.sprite = _on ? _onSprite : _offSprite;
    }

    public void SetToggle(bool on)
    {
        if (_on != on)
            onToggledEvent?.Invoke(on);

        _on = on;
        if (_image != null)
            _image.sprite = _on ? _onSprite : _offSprite;
    }
}
