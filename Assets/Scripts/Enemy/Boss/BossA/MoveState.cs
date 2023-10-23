using Enemy;
using Helper;
using UnityEngine;

namespace Enemy
{
    namespace BossA
    {
        public class MoveState : MonoBehaviour, IState
        {
            [SerializeField] float _moveSpeed = 5f;
            [SerializeField] private BossA_StateMachine _subject;
            private Vector2 _nextPosition;
            private static readonly float bottomPositionBound = 0.6f;
            private static readonly float topPositionBound = 0.8f;
            private static readonly float leftPositionBound = 0.2f;
            private static readonly float rightPositionBound = 0.8f;
            private static readonly float middlePositionBound = 0.5f;
            public void OnStateEnter()
            {
                bool moveToLeft = false;

                bool isOutLeftSide = transform.position.x < Cam.WorldLeft();
                bool isOutRightSide = transform.position.x > Cam.WorldRight();
                if (isOutLeftSide)
                {
                    // When the boss not inside camera view
                    moveToLeft = true;
                }
                else if (isOutRightSide)
                {
                    // When the boss not inside camera view
                    moveToLeft = false;
                }
                else
                {
                    // When the boss is inside camera view
                    // When boss is inside camera view
                    moveToLeft = _subject.transform.position.x > Cam.WorldPos().x;
                }
               

                if (moveToLeft)
                {
                    // move to the left side
                    _nextPosition = Cam.GetRandomPositionInRect(leftPositionBound, middlePositionBound, topPositionBound, bottomPositionBound);
                }
                else
                {
                    // move to the right side
                    _nextPosition = Cam.GetRandomPositionInRect(middlePositionBound, rightPositionBound, topPositionBound, bottomPositionBound);
                }
            }
            public void UpdateExecute()
            {}
            public void FixedUpdateExecute()
            {
                float distance = Vector2.Distance(_subject.transform.position, _nextPosition);
                if (!Mathf.Approximately(distance, 0f))
                {
                    Vector2 nextPos = Vector2.MoveTowards(_subject.transform.position, _nextPosition, _moveSpeed * Time.fixedDeltaTime);
                    _subject.LookAtTarget();
                    _subject.RigidBody.MovePosition(nextPos);
                }
                else
                {
                    _subject.ChangeState(_subject.DecideState);
                }
            }
            public void OnStateExit()
            {
                // Rotate to target
                var direction = _subject.Target.transform.position - _subject.transform.position; direction.z = 0f;
                _subject.transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
            }
        }

    }
}
