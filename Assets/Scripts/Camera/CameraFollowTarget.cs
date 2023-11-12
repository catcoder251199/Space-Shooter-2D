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
    }

    private void LateUpdate()
    {
        ProcessFollowTarget();
    }

    public void ProcessFollowTarget()
    {
        if (_targetRb == null)
            return;
        Vector2 currentCamPosition = _camRb.position;
        float left = currentCamPosition.x - _limitCameraSize.Width / 2;
        float right = currentCamPosition.x + _limitCameraSize.Width / 2;
        float top = currentCamPosition.y + _limitCameraSize.Height / 2;
        float bottom = currentCamPosition.y - _limitCameraSize.Height / 2;

        Vector2 targetPosition = _targetRb.position;
        float dX = 0;
        float dY = 0;

        if (targetPosition.x < left)
            dX = targetPosition.x - left;
        if (targetPosition.x > right) 
            dX = targetPosition.x - right;
        if (targetPosition.y > top)
            dY = targetPosition.y - top;
        if (targetPosition.y < bottom) 
            dY = targetPosition.y - bottom;

        Vector2 destination = currentCamPosition + new Vector2(dX, dY);
        Vector2 nextPosition = Vector2.MoveTowards(currentCamPosition, destination, _followSpeed * Time.fixedDeltaTime);
        _camRb.MovePosition(nextPosition);
    }
}
