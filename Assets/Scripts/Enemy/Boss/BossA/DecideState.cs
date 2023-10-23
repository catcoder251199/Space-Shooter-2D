using Enemy;
using UnityEngine;

namespace Enemy
{
    namespace BossA
    {
        public class DecideState : MonoBehaviour, IState
        {
            [SerializeField] private BossA_StateMachine _subject;

            public void OnStateEnter()
            {
                Debug.Log("BossA.DecideState Enter");
                _subject.ChangeToNextAttackState();
            }
            public void UpdateExecute() { }

            public void FixedUpdateExecute() { }
           
            public void OnStateExit() {
                Debug.Log("BossA.DecideState Exit");

            }
        }

    }
}
