using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiringEnemy : MonoBehaviour
{
    private GameObject _target;

    private void Start()
    {
        _target = GameObject.FindGameObjectWithTag("Player");
    }

    private void SpawnOutOfScreen()
    {

    }

    private void MoveToRandomPosition()
    {

    }
}
