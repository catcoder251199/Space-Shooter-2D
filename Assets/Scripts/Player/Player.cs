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
    [SerializeField] private List<AttackBehaviourSO> _attackBehaviourData;
    [SerializeField] private AttackBehaviour _attackBehaviour;

    [SerializeField] private Health _health;
    public Health Health => _health;

    private void Start()
    {
        _attackBehaviour.StartDoing();
    }
    public AttackBehaviourSO GetAttackBehaviourData(AttackBehaviourType type) => _attackBehaviourData[(int) type];

    public bool IsAlive() => true;
}
