using Enemy;
using UnityEngine;

namespace Enemy
{
    namespace BossA
    {
        public class StartState : MonoBehaviour, IState
        {
            [SerializeField] float _startMoveSpeed = 12f;
            [SerializeField] private BossA_StateMachine _subject;
            [SerializeField] float _delayOnExit = 2f;
            [SerializeField] private bool updateEnabled = false;

            private Vector2 endingPosition; 

            public void OnStateEnter()
            {
                if (_subject == null) return;

                float topOffset = 5f;
                // Determine the position at the top of the camera's view area as the starting point
                Vector3 startPosition = Helper.Cam.WorldPos() + new Vector3(0, Helper.Cam.HalfHeight() + topOffset, 0); startPosition.z = 0;
                _subject.transform.position = startPosition;
                // Determine the position at the middle of the camera's view area as the ending point
                endingPosition = Helper.Cam.GetWorldPositionInViewSpace(0.5f, 0.8f);
                _subject.SetShieldEnabled(true);
                updateEnabled = true;

                var gm = GameManager.Instance;
                gm.UIManager.UpdateBossHealth(_subject.Health.GetHealth(), _subject.Health.GetMaxHealth());
                gm.UIManager.ShowBossHealth();
            }
            public void UpdateExecute()
            {}
            public void FixedUpdateExecute()
            {
                if (!updateEnabled) return;

                if (_subject == null) return;

                float distance = Vector2.Distance(_subject.RigidBody.position, endingPosition);
                if (!Mathf.Approximately(distance, 0f))
                {
                    Vector2 nextPosition = Vector2.MoveTowards(_subject.RigidBody.position, endingPosition, _startMoveSpeed * Time.fixedDeltaTime);
                    _subject.RigidBody.MovePosition(nextPosition);
                }
                else
                {
                    updateEnabled = false;
                    Invoke("OnDestinationReached", _delayOnExit);
                }
            }

            public void OnDestinationReached()
            {
                _subject.SetShieldEnabled(false);
                _subject.ChangeState(_subject.DecideState);
            }

            public void OnStateExit()
            {
            
            }
        }

    }
}
