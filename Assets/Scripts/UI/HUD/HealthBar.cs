using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _hpText;
    [SerializeField] private Slider _slider;
    [SerializeField] private Image _fill;
    [SerializeField] private Gradient _gradient;
    public void SetHpMax(int max)
    {
        _slider.maxValue = max;
        if (_slider.value > _slider.maxValue)
        {
            _slider.value = _slider.maxValue;
            _hpText.text = ((int)_slider.maxValue).ToString();
        }
        _fill.color = _gradient.Evaluate(_slider.normalizedValue);
    }
    public void SetHpValue(int value)
    {
        value = Mathf.Clamp(value, 0, (int)_slider.maxValue);
        _slider.value = value;
        _hpText.text = ((int)_slider.value).ToString();
        _fill.color = _gradient.Evaluate(_slider.normalizedValue);
    }
}
