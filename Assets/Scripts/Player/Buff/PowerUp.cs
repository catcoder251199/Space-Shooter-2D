using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] BuffSO _buff;
    [SerializeField] string _pickableTag;
    [SerializeField] Vector2 _moveVec;
    [SerializeField] float _rotateSpeed = 0;

    Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        _rb.velocity = _moveVec;
        _rb.angularVelocity = _rotateSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(_pickableTag))
        {
            BuffableEntity entity = collision.attachedRigidbody.GetComponent<BuffableEntity>();
            if (entity != null)
            {
                entity.AddBuff(_buff.Initialize(collision.attachedRigidbody.gameObject));
                Destroy(gameObject);
            }
        }
    }
}
