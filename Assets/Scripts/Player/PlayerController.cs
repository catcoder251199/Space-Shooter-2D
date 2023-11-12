using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _speed = 1f;
    [SerializeField] private Vector2 _worldCenter;
    [Serializable] private struct MySize
    {
        public float Width;
        public float Height;
    }
    [SerializeField] private MySize _worldSize;
    private Vector2  _moveInput;
   
    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private float WorldLeft => _worldCenter.x - _worldSize.Width / 2;
    private float WorldRight => _worldCenter.x + _worldSize.Width / 2;
    private float WorldTop => _worldCenter.y + _worldSize.Height / 2;
    private float WorldBottom => _worldCenter.y - _worldSize.Height / 2;

    public void SetSpeed(float speed) => _speed = speed;
    struct mySize { public float Width; public float Height; };

    void FixedUpdate()
    {
        if (_rb != null)
        {
            Vector2 nextPosition = _rb.position + _moveInput * _speed * Time.fixedDeltaTime;
            nextPosition.x = Mathf.Clamp(nextPosition.x, WorldLeft, WorldRight);
            nextPosition.y = Mathf.Clamp(nextPosition.y, WorldBottom, WorldTop);
            _rb.MovePosition(nextPosition);
        }
    }

    void OnMove(InputValue value) // Read input
    {
        _moveInput = value.Get<Vector2>(); // normalized value
    }
}
