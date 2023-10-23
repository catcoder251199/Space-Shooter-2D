using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class FSMEnemy : MonoBehaviour
    {
        protected IState _currentState;

        public virtual void ChangeState(IState _nextState)
        {
            if (_nextState == null)
                return;

            _currentState?.OnStateExit();
            _currentState = _nextState;
            _currentState?.OnStateEnter();
        }
    }
}