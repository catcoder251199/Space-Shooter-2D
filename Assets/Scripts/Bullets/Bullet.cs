using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : BulletBase
{
    [SerializeField] private ParticleSystem _hitVFX;
    [SerializeField] private float _screenOffset = 10f;
    Rigidbody2D _rb;


    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        _rb.velocity = transform.up * _speed;

#if UNITY_EDITOR
        if (IsOutOfScreen())
        {
            Destroy(gameObject);
        }
#endif
    }

    bool IsOutOfScreen()
    {
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
        return screenPosition.x + _screenOffset < 0 || screenPosition.x - _screenOffset > Screen.width
            || screenPosition.y + _screenOffset < 0 || screenPosition.y - _screenOffset > Screen.height;
    }

    public override void TriggerHitVFX()
    {
        if (_hitVFX != null ) 
            Instantiate(_hitVFX, transform.position, Quaternion.identity, PlaySceneGlobal.Instance.VFXParent);
    }

#if !UNITY_EDITOR
    private void OnBecameVisible()
    {
        Destroy(gameObject);
    }
#endif
}
