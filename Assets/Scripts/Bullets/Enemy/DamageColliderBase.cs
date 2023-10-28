using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public abstract class DamageColliderBase : MonoBehaviour
{
    public string[] damageableTags;
    public int damage = 0;
    public bool isCritical = false;

    protected HashSet<string> _damageableTagsSet = new HashSet<string>(); // empty set

    protected virtual void Awake()
    {
        Init();
    }

    protected virtual void Init()
    {
        if (damageableTags != null)
        {
            foreach(var tag in damageableTags)
                _damageableTagsSet.Add(tag);
        }
    }

    public virtual bool CompareDamageableTag(string other)
    {
        return _damageableTagsSet.Contains(other);
    }
}
