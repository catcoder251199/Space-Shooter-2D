using UnityEngine;
using UnityEngine.Events;

using System.Collections;

namespace Enemy
{
    namespace BossA
    {
        public class AttackStateC : MonoBehaviour, IState
        {
            public UnityEvent OnAttackFinishedEvent;

            [SerializeField] private RotatingBullet _bulletPrefab;
            [SerializeField] private int _attackCount = 3;
            [SerializeField] private Transform _barrelTransform;
            [SerializeField] private float _delayAttack = 1f;
            [SerializeField] private BossA_StateMachine _subject;
            private Coroutine _shootRoutine;

            public void StartAttack()
            {
                _shootRoutine = StartCoroutine(FiringRoutine());
            }

            public float GetScale()
            {
                int enragedCount = _subject.GetEnragedCount();
                if (enragedCount <= 0)
                    return 2;
                else if (enragedCount == 1)
                {
                    return 2.5f;
                }
                else if (enragedCount == 2)
                {
                    return 3;
                }
                else // >= 3
                {
                    return 4;
                }
            }

            public int GetAttackCount()
            {
                return _attackCount + 2 * _subject.GetEnragedCount();
            }

            private IEnumerator FiringRoutine()
            {
                float scaleXY = GetScale();
                float attackCount = GetAttackCount();
                for (int i = 0; i < attackCount; i++)
                {
                    var bullet = Instantiate(_bulletPrefab, _barrelTransform.position, _barrelTransform.rotation, PlaySceneGlobal.Instance.BulletParent);
                    bullet.transform.localScale = new Vector3(scaleXY, scaleXY, 1f);
                    yield return new WaitForSeconds(_delayAttack);
                }
                yield return new WaitForSeconds(1f);
                _subject.ChangeState(_subject.MoveState);
                _shootRoutine = null;
            }

            public void OnStateEnter()
            {
                Debug.Log("BossA.AttackStateC Enter");
                StartAttack();
            }
            public void UpdateExecute() { }
            public void FixedUpdateExecute()
            {
                _subject.LookAtTarget();
            }
            public void OnStateExit() 
            {
                if (_shootRoutine != null)
                    StopCoroutine(_shootRoutine);
                Debug.Log("BossA.AttackStateC Exit");

            }
        }
    }
}