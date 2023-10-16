using System;
using UnityEngine;

public abstract class LaserGunBase : MonoBehaviour
{

    // * Shoot one laser beam per one activated time
    public abstract void Activate();
}