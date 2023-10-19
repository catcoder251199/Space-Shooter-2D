using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagableExplosion : MonoBehaviour
{
    [SerializeField] private int _damage;
    private float _destroyAfter = 1f;
    private float _colliderLifeTime = 0.5f;
    CircleCollider2D _collider;
    //[SerializeField] ParticleSystem _explosionVFX;
    public void SetDamage(int value) => _damage = value;

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(PlaySceneGlobal.Instance.Tag_Player))
        {
            Player player = collision.attachedRigidbody.GetComponent<Player>();
            player.TakeDamage(_damage, true);
        }
    }
}
