using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Single Shot SO", menuName = "ScriptableObjects/Attack Behaviour/SingleShot")]
public class SingleShotBehaviourSO : AttackBehaviourSO
{
    [SerializeField] float _fireRate = 1f;
    public float fireRate => _fireRate;
}
