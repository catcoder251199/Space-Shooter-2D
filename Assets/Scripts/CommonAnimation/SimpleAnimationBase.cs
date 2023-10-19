using System.Threading;
using UnityEngine;

public abstract class SimpleAnimationBase : MonoBehaviour
{
    [SerializeField] protected float _timer = 5.0f;
    protected float _countDownTime = 0f;
    [SerializeField] protected bool _runForever = false;
    public virtual void SetTimer(float timer) 
    {
        _timer = timer;
        _countDownTime = timer;
    }
    public virtual float GetTimer() => _timer;
    public virtual bool IsRunForever() => _runForever;
    public abstract void StartAnimationForever();
    public abstract void StartAnimationWithTimer();
    public abstract void StopAnimation();
    public abstract bool IsAnimating();
}