using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _speed = 1f;
    [SerializeField] private float _screenOffset = 10f;

    void Update()
    {
        transform.Translate(Vector2.up * _speed * Time.deltaTime);

#if UNITY_EDITOR
        if (IsOutOfScreen())
        {
            Vector2 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
            var check = screenPosition.x + _screenOffset < 0 || screenPosition.x - _screenOffset > Screen.width
            || screenPosition.y + _screenOffset < 0 || screenPosition.y - _screenOffset > Screen.height;
            Destroy(gameObject);
        }
    }
#endif

    bool IsOutOfScreen()
    {
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
        return screenPosition.x + _screenOffset < 0 || screenPosition.x - _screenOffset > Screen.width
            || screenPosition.y + _screenOffset < 0 || screenPosition.y - _screenOffset > Screen.height;
    }

#if !UNITY_EDITOR
    private void OnBecameVisible()
    {
        Destroy(gameObject);
    }
#endif
}
