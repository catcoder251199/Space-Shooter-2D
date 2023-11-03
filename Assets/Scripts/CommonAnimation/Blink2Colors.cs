using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

public class Blink2Colors : SimpleAnimationBase
{
    public event Action<Color> OnColorChanged;

    public float normalSpeed = 1.0f;
    public float fastSpeed = 2.0f;
    public Color fromColor = Color.white;
    public Color toColor = Color.white;
    public bool changeBlinkSpeedOnCountDown = false;
    public bool useCustomRenderer = false;
    private SpriteRenderer _spriteRenderer;
    private Color _oldColor;
    bool _isTriggered = false;
    public bool interpolate = true;
    private float _t = 0f; // * time have passed since animation has begun

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
    }
    void Update()
    {
        if (!_isTriggered)
            return;

        _t += Time.deltaTime;

        if (_runForever || (!_runForever && _countDownTime > 0f))
        {
            float blinkSpeed = normalSpeed;
            if (changeBlinkSpeedOnCountDown)
            {
                blinkSpeed = Mathf.Lerp(normalSpeed, fastSpeed, 1.0f - _countDownTime / _timer);
            }
            //float lerpValue = Mathf.PingPong(Time.time * blinkSpeed, 1.0f);
            float lerpValue = Mathf.PingPong(_t * blinkSpeed, 1.0f);
            Color lerpedColor = Color.Lerp(fromColor, toColor, interpolate ? lerpValue : (lerpValue >= 0.5f ? 1 : 0));

            if (!useCustomRenderer)
                _spriteRenderer.color = lerpedColor;
            else
            {
                // users have to change color by themselves
                OnColorChanged?.Invoke(lerpedColor);
            }

            if (!_runForever)
            {
                _countDownTime -= Time.deltaTime;
                if (_countDownTime <= 0f)
                    StopAnimation();
            }
        }
    }
    public void SetOldColor(Color color) => _oldColor = color;

    public override void StartAnimationForever()
    {
        _isTriggered = true;
        if (!useCustomRenderer)
            _oldColor = _spriteRenderer.color;
        _runForever = true;
    }
    public override void StartAnimationWithTimer()
    {
        _isTriggered = true;
        if (!useCustomRenderer)
            _oldColor = _spriteRenderer.color;
        _runForever = false;
        _countDownTime = _timer;
        _t = 0f;
    }
    public override void StopAnimation()
    {
        _isTriggered = false;
        if (!useCustomRenderer)
            _spriteRenderer.color = _oldColor;
        _t = 0f;
        _countDownTime = -1f;
    }

    public override bool IsAnimating()
    {
        return _isTriggered;
    }
}


