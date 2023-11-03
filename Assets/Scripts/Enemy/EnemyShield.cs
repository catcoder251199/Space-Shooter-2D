using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyShield : MonoBehaviour
{
    //[SerializeField] private bool _isRotated = true;
    [SerializeField] private float _rotateSpeed = 30f;
    //[SerializeField] private bool _clockwiseRotate = true;


    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        transform.Rotate(new Vector3(0, 0, _rotateSpeed * Time.deltaTime));
    }

    void FixedUpdate()
    {
        //if (_isRotated)
        //{
        //    _rb.angularVelocity = (_clockwiseRotate ? -1f : 1f) * _rotateSpeed;
        //}
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ProcessCollision(collision);
    }


    void ProcessCollision(Collider2D collision)
    {
        //if (collision.CompareTag(PlaySceneGlobal.Instance.Tag_PlayerBullet))
        //{
        //    var bullet = collision.attachedRigidbody.GetComponent<BulletBase>();
        //    if (bullet != null)
        //        bullet.TriggerHitVFX();
        //    Destroy(bullet.gameObject);
        //}
    }
}
