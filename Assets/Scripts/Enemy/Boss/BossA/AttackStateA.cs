using UnityEngine;
using UnityEngine.Events;

using System.Collections;

namespace Enemy
{
    namespace BossA 
    {
        public class AttackStateA : MonoBehaviour, IState
        {
            public UnityEvent OnAttackFinishedEvent;

            [SerializeField] private StraightBullet _bulletPrefab;
            [SerializeField] private int _attackCount = 3;
            [SerializeField] private Transform _barrelTransform;
            [SerializeField] private float _delayAttack = 1f;
            [SerializeField] private float _exitDelay = 1f;
            [SerializeField] private BossA_StateMachine _subject;
            private Coroutine _shootRoutine;
            public void StartAttack()
            {
                _shootRoutine = StartCoroutine(FiringRoutine());
            }

            public int GetAttackCount()
            {
                return _attackCount + 2 * _subject.GetEnragedCount();
            }

            private IEnumerator FiringRoutine()
            {
                int attackCount = GetAttackCount();
                for (int i = 0; i < attackCount; i++)
                {
                    //Instantiate(_bulletPrefab, _barrelTransform.position, _barrelTransform.rotation, PlaySceneGlobal.Instance.BulletParent);
                    var bullet = BulletFactory.Instance.CreateBulletProduct(_bulletPrefab.gameObject.GetInstanceID());
                    if (bullet == null)
                    {
                        Debug.LogError("AttackStateA: created bullet is null");
                        break;
                    }
                    bullet.transform.position = _barrelTransform.position;
                    bullet.transform.rotation = _barrelTransform.rotation;
                    bullet.transform.parent = PlaySceneGlobal.Instance.BulletParent;

                    yield return new WaitForSeconds(_delayAttack);
                }
                yield return new WaitForSeconds(_exitDelay);
                _subject.ChangeState(_subject.MoveState);
                _shootRoutine = null;
            }

            public void OnStateEnter()
            {
                Debug.Log("BossA.AttackStateA Enter");
                StartAttack();
            }
            public void UpdateExecute() { }
            public void FixedUpdateExecute() 
            {
                _subject.LookAtTarget();
            }
            public void OnStateExit() 
            {
                if (_shootRoutine != null )
                {
                    StopCoroutine(_shootRoutine);
                }
                Debug.Log("BossA.AttackStateA Exit");
            }
        }
    }
}