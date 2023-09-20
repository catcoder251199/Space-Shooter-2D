using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Meteor : MonoBehaviour
{
    [SerializeField] float _floatingSpeedMin = 1f;
    [SerializeField] float _floatingSpeedMax = 1.5f;
    float _floatingSpeed = 1f;

    [SerializeField] GameObject[] _meteorVisuals;
    GameObject _visual;

    [SerializeField] float _rotateSpeed = 360f;
    [SerializeField] float _offsetFromBounds = 2f; // in pixels
    Vector2 _moveDir;
    Rigidbody2D _rb2D;
    Vector2 _targetPosition;
    float _estimatedFlyToTime = -1f;
    float _floatingTime = 0f;
    private void Awake()
    {
        _rb2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        if (_visual == null)
            _visual = Instantiate(_meteorVisuals[Random.Range(0, _meteorVisuals.Length - 1)], Vector3.zero, Quaternion.identity, this.transform);

        _floatingSpeed = Random.Range(_floatingSpeedMin, _floatingSpeedMax);

        bool startFromLeft = Random.value > 0.5f;
        Vector2 startPosition;
        if (startFromLeft)
        {
            startPosition = Helper.Cam.GetLeftSideRandomPos(_offsetFromBounds, 0f, 0.35f, 1f);
            _targetPosition = Helper.Cam.GetRightSideRandomPos(_offsetFromBounds, 0f, 0f, 0.8f);
        }
        else // start from the right
        {
            startPosition = Helper.Cam.GetRightSideRandomPos(_offsetFromBounds, 0f, 0.35f, 1f);
            _targetPosition = Helper.Cam.GetLeftSideRandomPos(_offsetFromBounds, 0f, 0f, 0.8f);
        }
        _moveDir = (_targetPosition - startPosition).normalized;
        this.transform.position = startPosition;
        _rb2D.velocity = _moveDir * _floatingSpeed;
        _estimatedFlyToTime = Vector2.Distance(_targetPosition, startPosition) / _floatingSpeed;
        _floatingTime = 0f;
        _rb2D.angularVelocity = _rotateSpeed * Random.Range(-1f, 1f);
    }

    private void FixedUpdate()
    {
        if (!IsAlive())
        {
            Destroy(this.gameObject);
            return;
        }

        _floatingTime += Time.fixedDeltaTime;
        var distance = Vector2.Distance(_rb2D.position, _targetPosition);
        if (distance < 0.1f || _floatingTime >= _estimatedFlyToTime)
        {
            Initialize();
        }
    }

    private bool IsAlive() => true;

    private void OnTriggerEnter2D(Collider2D collision)
    {

    }
}
