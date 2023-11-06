using UnityEngine;
using UnityEngine.Events;

using System.Collections;

namespace Enemy
{
    namespace BossA 
    {
        public class AttackStateB : MonoBehaviour, IState
        {
            public UnityEvent OnAttackFinishedEvent;

            [SerializeField] private StraightBullet _bulletPrefab;
            [SerializeField] private int _attackCount = 2;
            [SerializeField] private int _bulletsPerShot = 3;
            [SerializeField] private float _bulletGap = 1f;
            [SerializeField] private Transform _barrelTransform;
            [SerializeField] private float _firingRate = 1.5f; // seconds per bullet
            [SerializeField] private BossA_StateMachine _subject;
            private Coroutine _shootRoutine;

            public void StartAttack()
            {
                _shootRoutine = StartCoroutine(FiringRoutine());
            }

            public int GetBulletPerShot()
            {
                int enragedCount = _subject.GetEnragedCount();
                if (enragedCount <= 0)
                    return _bulletsPerShot;
                else if (enragedCount == 1)
                {
                    return _bulletsPerShot + 2;
                }
                else if (enragedCount == 2)
                {
                    return _bulletsPerShot + 4;
                }
                else // >= 3
                {
                    return _bulletsPerShot + 6;
                }
            }

            public int GetBulletCountInArray()
            {
                int enragedCount = _subject.GetEnragedCount();
                if (enragedCount <= 0)
                    return 3;
                else if (enragedCount == 1)
                {
                    return 4;
                }
                else if (enragedCount == 2)
                {
                    return 5;
                }
                else // >= 3
                {
                    return 6;
                }
            }

            public float GetBulletSpreadAngle()
            {
                int enragedCount = _subject.GetEnragedCount();
                if (enragedCount <= 0)
                    return 30;
                else if (enragedCount == 1)
                {
                    return 45;
                }
                else if (enragedCount == 2)
                {
                    return 60;
                }
                else // >= 3
                {
                    return 90;
                }
            }

            public int GetAttackCount()
            {
                return _attackCount + _subject.GetEnragedCount() > 0 ? 1 : 0;
            }

            private IEnumerator FiringRoutine()
            {
                for (int i = 0; i < GetAttackCount(); i++)
                {
                    FireFanShot();
                    yield return new WaitForSeconds(_firingRate);
                }
                yield return new WaitForSeconds(1f);
                _subject.ChangeState(_subject.MoveState);
                _shootRoutine = null;
            }

            private void FireFanShot()
            {
                float bulletSpreadAngle = GetBulletSpreadAngle();
                int bulletPerShot = GetBulletPerShot();
                int bulletCountInArray = GetBulletCountInArray();

                float angleStep = bulletSpreadAngle / (bulletPerShot - 1);
                float startAngle = transform.rotation.eulerAngles.z - bulletSpreadAngle / 2;
                Vector3 startPosition = _barrelTransform.position;
                for (int i = 0; i < bulletPerShot; i++)
                {
                    float angle = startAngle + i * angleStep;
                    Quaternion bulletRotation = Quaternion.Euler(0, 0, angle);
                    for (int j = 0; j < bulletCountInArray; j++)
                    {
                        //var bullet = Instantiate(_bulletPrefab, _barrelTransform.position, bulletRotation, PlaySceneGlobal.Instance.BulletParent);
                        var bullet = BulletFactory.Instance.CreateBulletProduct(_bulletPrefab.gameObject.GetInstanceID())?.GetComponent<StraightBullet>();
                        if (bullet == null)
                        {
                            Debug.LogError("AttackStateB: created bullet is null");
                            break;
                        }
                        bullet.transform.rotation = bulletRotation;
                        bullet.transform.position = _barrelTransform.position + bullet.transform.up * j * _bulletGap;
                        bullet.transform.parent = PlaySceneGlobal.Instance.BulletParent;
                    }
                }
            }
            public void OnStateEnter()
            {
                Debug.Log("BossA.AttackStateB Enter");
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
                Debug.Log("BossA.AttackStateB Exit");
            }
        }
    }
}