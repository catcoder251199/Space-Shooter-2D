using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyShield : MonoBehaviour
{
    [SerializeField] private float _rotateSpeed = 30f;
    private void Awake()
    {
    }

    void Update()
    {
        transform.Rotate(new Vector3(0, 0, _rotateSpeed * Time.deltaTime));
    }
}
