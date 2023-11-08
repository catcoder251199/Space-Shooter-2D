using UnityEngine;

public abstract class TimedBuff
{
    protected float _duration;
    protected int _effectStacks;
    protected bool _isFinished;
    protected GameObject _obj; // the actor get buffed
    public BuffSO Buff { get; }
    public bool IsFinished => _isFinished;
    public TimedBuff(BuffSO buff, GameObject gameObject)
    {
        Buff = buff;
        _obj = gameObject;
    }

    public void Tick(float delta)
    {
        if (Buff.isForever)
            return;

        _duration -= delta;
        if (_duration <= 0)
        {
            OnFinished();
            _duration = 0;
            _isFinished = true;
        }
    }

    public void Activate()
    {
        OnActivated();
        if (Buff.isForever)
        {
            if (Buff.isEffectStacked || (!Buff.isEffectStacked && _effectStacks <= 0))
            {
                ApplyEffect();
                _effectStacks += 1;
            }
        }
        else
        {
            if (_duration <= 0)
            {
                _duration = Mathf.Max(0, _duration); // make it positive
                _duration += Buff.duration;
                ApplyEffect();
                _effectStacks++;
            }
            else
            {
                if (Buff.isDurationStacked)
                {
                    _duration = Mathf.Max(0, _duration); // make it positive
                    _duration += Buff.duration;
                }
                if (Buff.isEffectStacked)
                {
                    ApplyEffect();
                    _effectStacks++;
                }
            }
        }
    }

    // Do something when the effect expires, it might include removing applied effects 
    public abstract void OnFinished();

    // Should call once whenever the actor get a buff.
    // Only call this through Activate() method
    protected abstract void ApplyEffect();
    protected virtual void OnActivated() { } // whenever the buff is granted (but it might have no effect)
    
}
