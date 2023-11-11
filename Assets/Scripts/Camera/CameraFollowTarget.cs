using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowTarget : MonoBehaviour
{
    [SerializeField, Header("Follow Target")] Rigidbody2D _targetRb;
    [SerializeField] private float _followSpeed = 1.0f;
    [Serializable] struct MySize 
    {
        public float Width;
        public float Height;
    }
    [SerializeField] private MySize _limitCameraSize;
    private Rigidbody2D _camRb;

    void Awake()
    {
        _camRb = GetComponent<Rigidbody2D>();
    }
    public void SetTarget(Rigidbody2D target)
    {
        _targetRb = target;
    }

    void FixedUpdate()
    {
        ProcessFollowTarget();
    }

    private void LateUpdate()
    {
        
    }

    private void ProcessFollowTarget()
    {
        if(_camRb == null) 
            return;

        float LimitLeft = _camRb.position.x - _limitCameraSize.Width / 2;
        float LimitRight = _camRb.position.x + _limitCameraSize.Width / 2;
        float LimitTop = _camRb.position.y + _limitCameraSize.Height / 2;
        float LimitBottom = _camRb.position.y - _limitCameraSize.Height / 2;

        Vector2 camMoveDirection = Vector2.zero;

        if (_targetRb.position.x < LimitLeft)
            camMoveDirection.x = _targetRb.position.x - LimitLeft;
        if (_targetRb.position.x > LimitRight)
            camMoveDirection.x = _targetRb.position.x - LimitRight;
        if (_targetRb.position.y > LimitTop)
            camMoveDirection.y = _targetRb.position.y - LimitTop;
        if (_targetRb.position.y < LimitBottom)
            camMoveDirection.y = _targetRb.position.y - LimitBottom;

        Vector2 nextPosition = _camRb.position + camMoveDirection.normalized * Time.fixedDeltaTime * _followSpeed;
        _camRb.MovePosition(nextPosition);
    }
}
