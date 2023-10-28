using UnityEngine;

public abstract class AutoShootDevice : MonoBehaviour
{
    public abstract bool IsActivate();
    public abstract void Activate();
    public abstract void Deactivate();

    public abstract float GetDelayStart();
}