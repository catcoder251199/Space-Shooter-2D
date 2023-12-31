using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemy
{
    namespace LaserShooterState
    {
        public class StartState : IState
        {
            private LaserShooter _subject;
            private Vector3 _startPosition;

            public StartState(LaserShooter subject)
            {
                _subject = subject;
            }
            private Vector2 GetRandOffScreenPosition()
            {
                var sides = new Helper.Cam.Side[] {
            Helper.Cam.Side.Top,
            Helper.Cam.Side.Left,
            Helper.Cam.Side.Right};
                var randSide = sides[Random.Range(0, sides.Length)];
                Vector2 retPos = Vector2.zero;
                switch (randSide)
                {
                    case Helper.Cam.Side.Top:
                        retPos = Helper.Cam.GetRandomPosOnSide(randSide, _subject.OffsetFromBounds, 0, 0f, 1f); break;
                    case Helper.Cam.Side.Left:
                        retPos = Helper.Cam.GetRandomPosOnSide(randSide, _subject.OffsetFromBounds, 0, 0.7f, 0.95f); break;
                    case Helper.Cam.Side.Right:
                        retPos = Helper.Cam.GetRandomPosOnSide(randSide, _subject.OffsetFromBounds, 0, 0.7f, 0.95f); break;
                }
                return retPos;
            }
            public void OnStateEnter()
            {
                // Reset to initial state
                _subject.transform.position = Vector3.zero;
                _subject.transform.rotation = Quaternion.identity;
                _subject.Rigidbody.velocity = Vector3.zero;
                _subject.Rigidbody.angularVelocity = 0;

                // Setup before starting to move
                _subject.transform.position = GetRandOffScreenPosition();
                _startPosition = Helper.Cam.GetRandomPositionInRect(0.1f, 0.9f, 0.9f, 0.5f);
                var direction = (_startPosition - _subject.transform.position).normalized;
                _subject.transform.rotation = Quaternion.LookRotation(_subject.transform.forward, direction);
            }
            public void UpdateExecute() { }

            public void FixedUpdateExecute()
            {
                if (Vector2.Distance(_subject.transform.position, _startPosition) > Mathf.Epsilon)
                {
                    Vector2 nextPosition = Vector2.MoveTowards(_subject.Rigidbody.position, _startPosition, _subject.Speed * Time.fixedDeltaTime);
                    _subject.Rigidbody.MovePosition(nextPosition);
                }
                else
                {
                    _subject.ChangeState(_subject.WaitState);
                }
            }

            public void OnStateExit() { }
        }
    }
}
