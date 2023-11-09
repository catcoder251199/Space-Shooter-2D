using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PowerupUIItem : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _text;
    public BuffSO powerupBuff;
    public void UpdateUI(BuffSO buff)
    {
        _image.sprite = buff.sprite;
        _text.text = buff.buffName;
        powerupBuff = buff;
    }
}
