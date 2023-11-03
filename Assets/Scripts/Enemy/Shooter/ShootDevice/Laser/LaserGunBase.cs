using System;
using UnityEngine;

public abstract class LaserGunBase : MonoBehaviour
{
    protected Action _onAttackFinished;
    protected void TriggerOnAttackFinishedEvent()
    {
        _onAttackFinished?.Invoke();
    }

    //* OnFinished is one-called function
    public abstract void ActivateLaserBeam(float delay, bool blinkOnDelay, Action onFinished);
    public abstract void SetSightLineEnabled(bool enabled);
}