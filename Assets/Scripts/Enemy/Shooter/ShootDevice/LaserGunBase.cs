using System;
using UnityEngine;

public abstract class LaserGunBase : MonoBehaviour
{
    public enum Mode
    {
        None,
        Probe,
        Shoot
    }
    protected Mode _currentMode = Mode.None;
    public virtual bool IsInMode(Mode mode) => _currentMode == mode;

    public event Action OnAttackFinishedEvent;
    public void TriggerOnAttackFinishedEvent()
    {
        OnAttackFinishedEvent?.Invoke();
    }

    public abstract float GetDelayTime();
    public abstract void SwitchMode(Mode mode);

    // * Shoot one laser beam per one activated time
    public abstract void Activate();
}