using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerNS
{
    public interface IShootPattern
    {
        public void Start();
        public void Stop();
        public void OnRemoved();
        public void OnAdded();
        public bool IsShooting();
    }
}

