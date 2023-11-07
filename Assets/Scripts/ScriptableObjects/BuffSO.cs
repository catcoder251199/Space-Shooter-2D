using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BuffSO : ScriptableObject
{
    public float duration;
    public bool isDurationStacked;
    public bool isEffectStacked;
    public string description;

    [Tooltip("if it's true. duration and isDurationStacked are overridden and have no effect")]
    public bool isForever;

    public abstract TimedBuff Initialize(GameObject gameObject);
}
