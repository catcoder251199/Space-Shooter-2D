using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public enum AttackBehaviourType
    {
        Single = 0,
        Double,
        Splitter
    };

    [SerializeField] private PlayerController _controller;
    [SerializeField] private AttackBehaviour _attackBehaviour;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private List<AttackBehaviourSO> _attackBehaviourData;
    private void Start()
    {
        _attackBehaviour.StartDoing();
    }

    public GameObject GetBulletPrefab() => _bulletPrefab;
    public AttackBehaviourSO GetAttackBehaviourData(AttackBehaviourType type) => _attackBehaviourData[(int) type];
}
