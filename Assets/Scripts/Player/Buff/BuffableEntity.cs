using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffableEntity : MonoBehaviour
{
    private readonly Dictionary<BuffSO, TimedBuff> _buffDict = new Dictionary<BuffSO, TimedBuff>();

    private void Update()
    {
        var gm = GameManager.Instance;
        if (gm != null && gm.Paused)
            return;

        Queue<TimedBuff> removed = new Queue<TimedBuff>();

        foreach (var buff in _buffDict.Values)
        {
            if (buff != null && !buff.Buff.isForever)
            {
                buff.Tick(Time.deltaTime);
                if (buff.IsFinished)
                    removed.Enqueue(buff);
            }
        }

        while (removed.Count > 0)
        {
            var removedBuff = removed.Dequeue();
            _buffDict.Remove(removedBuff.Buff);
        }
    }

    public void AddBuff(TimedBuff buff)
    {
        if (_buffDict.ContainsKey(buff.Buff))
            _buffDict[buff.Buff].Activate();
        else
        {
            _buffDict[buff.Buff] = buff;
            buff.Activate();
        }
    }
}
