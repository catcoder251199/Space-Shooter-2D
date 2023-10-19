using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    private TextMeshPro _textMeshPro;
    [SerializeField] float _lifeTime = 1f;
    [SerializeField] float _disappearSpeed = 1f;
    [SerializeField] Color _normalColor;
    [SerializeField] Color _criticalColor;

    float _halfLifeTimeThreshHold = 1f;
    Vector3 minScale;
    public static DamagePopup Create(int damage, Vector3 worldPosition, bool isCritical)
    {
        // * Should optimize with Object Pooling Design
        GameObject popup = Instantiate(PlaySceneGlobal.Instance.DamagePopPrefab, worldPosition, Quaternion.identity, PlaySceneGlobal.Instance.VFXParent);
        DamagePopup damagePopup = popup.GetComponent<DamagePopup>();
        damagePopup.SetDamage(damage, isCritical);
        
        return damagePopup;
    }
    private void Awake()
    {
        _textMeshPro = GetComponent<TextMeshPro>();
        minScale = transform.localScale * 0.5f;
        _halfLifeTimeThreshHold = _lifeTime * 0.7f;
    }

    private void Update()
    {
        if (_lifeTime > _halfLifeTimeThreshHold)
        {
            float scaleAmount = 1f;
            transform.localScale += Vector3.one * scaleAmount * Time.deltaTime;
        }
        else
        {
            float scaleAmount = 0.8f;
            transform.localScale -= Vector3.one * scaleAmount * Time.deltaTime;
            transform.localScale = new Vector3(
                Mathf.Max(transform.localScale.x, minScale.x),
                Mathf.Max(transform.localScale.y, minScale.y),
                Mathf.Max(transform.localScale.z, minScale.z)
                );
        }

        if (_lifeTime > 0f)
        {
            var textColor = _textMeshPro.color;
            textColor.a -= _disappearSpeed * Time.deltaTime;
            _textMeshPro.color = textColor;
            _lifeTime -= Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetDamage(int damage, bool isCritical = false)
    {
        _textMeshPro.text = damage.ToString();
        if (isCritical)
        {
            _textMeshPro.fontSize = 10;
            _textMeshPro.color = _criticalColor;
        }
        else
        {
            _textMeshPro.fontSize = 8;
            _textMeshPro.color = _normalColor;
        }
    }
}
