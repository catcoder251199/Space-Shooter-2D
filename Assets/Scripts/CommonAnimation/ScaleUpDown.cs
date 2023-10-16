using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

public class ScaleUpDown : MonoBehaviour
{
    Coroutine _curRoutine;
    Vector3 oldScale;
    public bool LockX = false;
    public bool LockY = false;
    public bool LockZ = false;

    private void Start()
    {
        Trigger(0.74f, 0.75f, true, 60, 0.05f, 5f);
    }

    private IEnumerator ScaleUpDownRoutine(float initScale, float targetScale, bool upscale, float fps, float time, bool runForever = false, float timer = 0f)
    {

        float deltaTime = 1 / fps; // Time between frame
        //* the rate at which the scale changes over a time period
        float rate = (targetScale - initScale) / time; // time is the time to chane from initscale to targetscale
        float dScale = rate * deltaTime;
        float currentScale = initScale;

        oldScale = this.transform.localScale;


        Func<bool> ShouldStop = () =>
        {
            if (runForever)
            {
                return false;
            }
            else
                return timer <= 0;
        };
        while (!ShouldStop())
        {
            while (upscale)
            {
                yield return new WaitForSeconds(deltaTime);
                timer -= deltaTime;

                currentScale += dScale;
                if (currentScale > targetScale)
                {
                    upscale = false;
                    currentScale = targetScale;
                }
                Vector3 baseScale = Vector3.one * currentScale;
                if (LockX)
                    baseScale.x = oldScale.x;
                if (LockY)
                    baseScale.y = oldScale.y;
                if (LockZ)
                    baseScale.z = oldScale.z;

                transform.localScale = baseScale;
            }

            while (!upscale)
            {
                yield return new WaitForSeconds(deltaTime);
                timer -= deltaTime;

                currentScale -= dScale;
                if (currentScale < initScale)
                {
                    upscale = true;
                    currentScale = initScale;
                }
                Vector3 baseScale = Vector3.one * currentScale;
                if (LockX)
                    baseScale.x = oldScale.x;
                if (LockY)
                    baseScale.y = oldScale.y;
                if (LockZ)
                    baseScale.z = oldScale.z;
                transform.localScale = baseScale;
            }
        }
    
        StopAnimation();
    }

    public void Trigger(float initScale, float targetScale, bool upscale, float fps, float time, float timer)
    {
        if (!CanTrigger())
            return;
        _curRoutine = StartCoroutine(ScaleUpDownRoutine(initScale, targetScale, upscale, fps, time, false, timer));
    }

    public void TriggerForever(float initScale, float targetScale, bool upscale, float fps, float time)
    {
        if (!CanTrigger())
            return;
        _curRoutine = StartCoroutine(ScaleUpDownRoutine(initScale, targetScale, upscale, fps, time, true));
    }
    public bool CanTrigger()
    {
        return _curRoutine == null;
    }
    public void StopAnimation(bool backToOldScale = true)
    {
        StopCoroutine(_curRoutine);
        _curRoutine = null;
        this.transform.localScale = oldScale;
    }
}


