using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] BuffSO _buff;
    [SerializeField] string _pickableTag;
    [SerializeField] PooledSpawnableProduct _pooledProduct;
    public Vector2 moveVec;
    public float rotateSpeed = 0;

    Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    public void Initialize()
    {
        transform.localScale = Vector3.one;
        _rb.velocity = Vector2.zero;
        _rb.angularVelocity = 0;
    }

    public void Update()
    {
        if (!IsInScreen())
            Deactivate();
    }
    private bool IsInScreen()
    {
        return Helper.Cam.IsPositionInWorldCamRect(transform.position, 2f);
    }

    private void FixedUpdate()
    {
        _rb.velocity = moveVec;
        _rb.angularVelocity = rotateSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(_pickableTag))
        {
            BuffableEntity entity = collision.attachedRigidbody.GetComponent<BuffableEntity>();
            if (entity != null)
            {
                entity.AddBuff(_buff.Initialize(collision.attachedRigidbody.gameObject));
                Deactivate();
            }
        }
    }

    private void Deactivate()
    {
        if (_pooledProduct != null)
        {
            _pooledProduct.Release();
        }
        else
            Destroy(gameObject);
    }
}
