using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class FiringEnemy : MonoBehaviour
    {
        private Transform _target;
        [SerializeField] private float _speed = 3.0f;
        [SerializeField] private float _rotateSpeed = 30.0f;
        [SerializeField] private float _attackTime = 2.5f;
        [SerializeField] float _offsetFromBounds = 4f; // in pixels
        [SerializeField] bool _active = true;
        [SerializeField] Enemy.ShootingBehaviour _shootingBehaviour;

        private void Start()
        {
            _target = GameObject.FindGameObjectWithTag("Player").transform;
            transform.position = GetPositionToOutOfScreen();
            StartCoroutine(EnemyRoutine());
        }
        private Vector2 GetPositionToOutOfScreen()
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
                    retPos = Helper.Cam.GetRandomPosOnSide(randSide, _offsetFromBounds, 0, 0f, 1f); break;
                case Helper.Cam.Side.Left:
                    retPos = Helper.Cam.GetRandomPosOnSide(randSide, _offsetFromBounds, 0, 0.7f, 0.95f); break;
                case Helper.Cam.Side.Right:
                    retPos = Helper.Cam.GetRandomPosOnSide(randSide, _offsetFromBounds, 0, 0.7f, 0.95f); break;
            }
            return retPos;
        }
        private Vector2 GetRandomPositionInRect(float left, float right, float top, float bottom) // left, right, top, bottom are normalized
        {
            left = Mathf.Clamp01(left);
            right = Mathf.Clamp01(right);
            top = Mathf.Clamp01(top);
            bottom = Mathf.Clamp01(bottom);

            Camera cam = Camera.main;
            return cam.ViewportToWorldPoint(new Vector3(Random.Range(left, right), Random.Range(bottom, top), cam.nearClipPlane));
        }

        private IEnumerator EnemyRoutine()
        {
            // we're gonna spawn at position in the rect which is top half of camera viewport
            // that rect is split in 2 halves which are left side and right side
            // enemyship moves to between 2 sides back and forth 

            const float bottomPositionBound = 0.7f;
            const float topPositionBound = 0.95f;
            const float leftPositionBound = 0.1f;
            const float rightPositionBound = 0.95f;
            float middlePositionBound = Mathf.Clamp(0.5f, leftPositionBound, rightPositionBound);

            // Check if spawned position is near left side or right side
            var spawnedPosX = Camera.main.WorldToViewportPoint(transform.position).x;
            bool spawnInLeftSide = (spawnedPosX - middlePositionBound) < 0; // true -> left side. false  -> right side
                                                                            // get a random position on the left side as the beginning position
            Vector3 targetPosition = spawnInLeftSide ?
                GetRandomPositionInRect(leftPositionBound, middlePositionBound, topPositionBound, bottomPositionBound)
                : GetRandomPositionInRect(middlePositionBound, rightPositionBound, topPositionBound, bottomPositionBound);

            transform.rotation = Quaternion.LookRotation(Vector3.forward, targetPosition - transform.position);
            yield return StartCoroutine(MoveToRoutine(targetPosition, _speed)); // On spawned, enemy moves to the first position

            while (_active)
            {
                // rotate to look at player
                yield return StartCoroutine(RotateToRoutine(_target.transform.position, _rotateSpeed));
                yield return new WaitForSeconds(0.5f);

                // Attack the player
                yield return StartCoroutine(AttackRoutine());

                spawnInLeftSide = !spawnInLeftSide; // choose the other side to get the next random postion
                                                    // Choose a random next position
                var targetPos = spawnInLeftSide ?
                GetRandomPositionInRect(leftPositionBound, middlePositionBound, topPositionBound, bottomPositionBound)
                : GetRandomPositionInRect(middlePositionBound, rightPositionBound, topPositionBound, bottomPositionBound);

                yield return StartCoroutine(RotateToRoutine(targetPos, _rotateSpeed)); // look at to the next position
                                                                                       //yield return new WaitForSeconds(0.1f);

                yield return StartCoroutine(MoveToRoutine(targetPos, _speed)); // move to the next position
                                                                               //yield return new WaitForSeconds(0.1f);
            }
        }

        private IEnumerator MoveToRoutine(Vector3 targetPos, float speed)
        {
            while (Vector2.Distance(transform.position, targetPos) > Mathf.Epsilon)
            {
                transform.position = Vector2.MoveTowards(transform.position, targetPos, _speed * Time.deltaTime);
                yield return null;
            }
        }

        private IEnumerator RotateToRoutine(Vector3 targetPos, float rotateSpeed)
        {
            var directionToTarget = (targetPos - transform.position);
            var targetRotation = Quaternion.LookRotation(Vector3.forward, directionToTarget);
            while (transform.rotation != targetRotation)
            {
                yield return null;
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotateSpeed * Time.deltaTime);
            }
        }

        private IEnumerator AttackRoutine()
        {
            if (_shootingBehaviour)
            {
                _shootingBehaviour.StartAttack();
                yield return new WaitForSeconds(_attackTime);
                _shootingBehaviour.EndAttack();
            }
        }
    }
}
