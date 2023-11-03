using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagableExplosion : MonoBehaviour
{
    private float _destroyAfter = 1f;
    private float _colliderLifeTime = 0.5f;
    CircleCollider2D _collider;
    private void Awake()
    {
        _collider = GetComponent<CircleCollider2D>();
        _collider.enabled = true;
        //_explosionVFX.Play();
    }
    void Start()
    {
        Destroy(gameObject, _destroyAfter);
        StartCoroutine(StopHitOthers());
    }

    IEnumerator StopHitOthers()
    {
        yield return new WaitForSeconds(_colliderLifeTime);
        _collider.enabled = false;
    }
}
