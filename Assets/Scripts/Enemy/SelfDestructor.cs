using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SelfDestructor : MonoBehaviour
{
    [SerializeField] float _speed = 1f;
    [SerializeField] float _rotateSpeed = 90f;
    [SerializeField] float _rotateTime = 1f;
    [SerializeField] float _fromPlayerRadiusMin = 1f;
    [SerializeField] float _fromPlayerRadiusMax = 2f;
    [SerializeField] float _offsetFromBounds = 2f;
    [SerializeField] float _countDownTime = 5f;

    [SerializeField] GameObject _explosionVFX;

    private Transform _worldUI;
    [SerializeField] RectTransform _canvasUI;
    [SerializeField] TextMeshProUGUI _countDownText;

    private Vector3 _destination;
    private Transform _target;

    void Start()
    {
        _target = GameObject.FindGameObjectWithTag("Player").transform;
        _worldUI = GameObject.FindGameObjectWithTag("World UI").transform;
        _canvasUI.SetParent(_worldUI);
        
        StartCoroutine(MoveRoutine());
    }

    private void Update()
    {
        if (_countDownTime <= 0)
        {
            Destroy(gameObject);
            Destroy(_canvasUI.gameObject);
            Instantiate(_explosionVFX, this.transform.position, Quaternion.identity);
        }
        else
        {
            _countDownTime -= Time.deltaTime;
        }
    }

    private void LateUpdate()
    {
        _canvasUI.position = (Vector2) this.transform.position + new Vector2(0.5f, -0.5f);
        _countDownText.text = ((int) _countDownTime).ToString();
    }

    private Vector2 GetNextDestination()
    {
        return (Vector2)_target.position
            + Random.insideUnitCircle.normalized 
            * Random.Range(_fromPlayerRadiusMin, _fromPlayerRadiusMax);
    }
    private Vector2 GetStartPosition()
    {
        var sides = new Helper.Cam.Side[] {
            Helper.Cam.Side.Top,
            Helper.Cam.Side.Left,
            Helper.Cam.Side.Right};
        var randSide = sides[Random.Range(0, sides.Length)];
        Vector2 retPos = Vector2.zero;
        switch (randSide)
        {
            case Helper.Cam.Side.Top:
                retPos = Helper.Cam.GetRandomPosOnSide(randSide, _offsetFromBounds, 0, 0f, 1f); break;
            case Helper.Cam.Side.Left:
                retPos = Helper.Cam.GetRandomPosOnSide(randSide, _offsetFromBounds, 0, 0.7f, 0.95f); break;
            case Helper.Cam.Side.Right:
                retPos = Helper.Cam.GetRandomPosOnSide(randSide, _offsetFromBounds, 0, 0.7f, 0.95f); break;
        }
        return retPos;
    }

    private Vector2 GetRandomInsidePosition()
    {
        return Helper.Cam.GetRandomPositionInRect(0.1f, 0.9f, 0.9f, 0.5f);
    }

    IEnumerator MoveToScreenRoutine()
    {
        this.transform.position = GetStartPosition();
        Vector2 destination = GetRandomInsidePosition();

        while (Vector2.Distance(this.transform.position, destination) > Mathf.Epsilon)
        {
            this.transform.position =  Vector2.MoveTowards(this.transform.position, destination, _speed * Time.deltaTime);
            yield return null;
        }
    }

    IEnumerator WaitAndLookRoutine()
    {
        float rotatedTime = 0f;
        while (rotatedTime < _rotateTime)
        {
            var directionToTarget = _target.transform.position - this.transform.position;
            directionToTarget.z = this.transform.position.z; // Keep the z coord of this object
            var targetRotation = Quaternion.LookRotation(Vector3.forward, directionToTarget);
            this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, targetRotation, Time.deltaTime * _rotateSpeed);
            
            rotatedTime += Time.deltaTime;

            yield return null;
        }
    }
    IEnumerator MoveToDestinationRoutine()
    {
        while (Vector2.Distance(this.transform.position, _destination) > Mathf.Epsilon)
        {
            this.transform.position = Vector2.MoveTowards(this.transform.position, _destination, _speed * Time.deltaTime);
            yield return null;
        }
    }

    IEnumerator MoveRoutine()
    {
        yield return StartCoroutine(MoveToScreenRoutine());

        while (true)
        {
            yield return 0.2f;
            yield return StartCoroutine(WaitAndLookRoutine());
            yield return 0.2f;
            yield return StartCoroutine(MoveToDestinationRoutine());
            _destination = GetNextDestination();
        }
    }
}
