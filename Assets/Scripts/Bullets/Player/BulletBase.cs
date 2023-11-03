using UnityEngine;

public abstract class BulletBase : MonoBehaviour
{
    public abstract int Damage { get; set; }
    public abstract bool IsCritical { get; set; }
}