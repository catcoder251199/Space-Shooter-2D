using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleUpDown : SimpleAnimationBase
{
    public bool LockX = false;
    public bool LockY = false;
    public bool LockZ = false;

    public Vector3 maxScale = new Vector3(2f, 2f, 2f);
    public Vector3 minScale = new Vector3(0.5f, 0.5f, 0.5f);
    public float scalingSpeed = 1.0f;  // // Scaling speed (scale_units/seconds)

    private Vector3 _oldScale = Vector3.one;
    private bool _isScalingUp = true;
    private bool _isTriggered = false;

    private void Start()
    {
    }

    private void Update()
    {
        if (!_isTriggered)
            return;

        if (_runForever || (!_runForever && _countDownTime > 0f))
        {
            // Calculate the scale change based on the current direction
            float scaleFactor = (_isScalingUp) ? scalingSpeed : -scalingSpeed;

            // Get the current scale of the object
            Vector3 currentScale = transform.localScale;

            // Calculate the new scale
            Vector3 newScale = currentScale + new Vector3(scaleFactor, scaleFactor, scaleFactor) * Time.deltaTime;

            // Clamp the new scale within the specified maxScale and minScale
            newScale.x = Mathf.Clamp(newScale.x, minScale.x, maxScale.x);
            newScale.y = Mathf.Clamp(newScale.y, minScale.y, maxScale.y);
            newScale.z = Mathf.Clamp(newScale.z, minScale.z, maxScale.z);

            if (LockX)
                newScale.x = transform.localScale.x;
            if (LockY)
                newScale.y = transform.localScale.y;
            if (LockZ)
                newScale.z = transform.localScale.z;

            // Assign the new scale to the object
            transform.localScale = newScale;

            // Reverse direction when reaching the max or min limit
            if (newScale == maxScale || newScale == minScale)
            {
                _isScalingUp = !_isScalingUp;
            }

            if (!_runForever)
            {
                _countDownTime -= Time.deltaTime;
                if (_countDownTime <= 0f)
                    StopAnimation();
            }
        }
    }
    public override void StartAnimationForever()
    {
        _isTriggered = true;
        _oldScale = transform.localScale;
        _runForever = true;
    }
    public override void StartAnimationWithTimer()
    {
        _isTriggered = true;
        _oldScale = transform.localScale;
        _runForever = false;
        _countDownTime = _timer;
    }
    public override void StopAnimation()
    {
        _isTriggered = false;
        transform.localScale = _oldScale;
        _countDownTime = -1f;
    }

    public override bool IsAnimating()
    {
        return _isTriggered;
    }
}
