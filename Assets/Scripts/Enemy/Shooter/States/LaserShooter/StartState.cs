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
                _subject.transform.position = GetRandOffScreenPosition();
                _startPosition = Helper.Cam.GetRandomPositionInRect(0.1f, 0.9f, 0.9f, 0.5f);
                var moveDir = _startPosition - _subject.transform.position; moveDir.z = _subject.transform.position.z;
                _subject.transform.rotation = Quaternion.LookRotation(_subject.transform.forward, moveDir);
            }
            public void Execute()
            {
                if (Vector2.Distance(_subject.transform.position, _startPosition) > Mathf.Epsilon)
                {
                    _subject.transform.position = Vector2.MoveTowards(
                        _subject.transform.position,
                        _startPosition,
                        Time.deltaTime * _subject.Speed);
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
