using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float speed = 1f;
    Vector2 moveInput;

    Vector2 cameraBound;
    void Start()
    {
        
    }

    void Update()
    {
        MovePerFrame();
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void MovePerFrame()
    {
        transform.Translate(moveInput * speed * Time.deltaTime, Space.World);
    }

}
