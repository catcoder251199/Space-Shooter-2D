using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public abstract class ShootingBehaviour : MonoBehaviour
    {
        public abstract void StartAttack();

        public abstract void EndAttack();
    }
}


