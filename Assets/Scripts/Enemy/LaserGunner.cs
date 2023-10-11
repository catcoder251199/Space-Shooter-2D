using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LaserGunner : MonoBehaviour
{
    private Transform _target;
    private Vector2 _targetPosition;

    [SerializeField] private float _speed = 1f;
    [SerializeField] private float _rotateSpeed = 90f;
    [SerializeField] private float _rotateTime = 3f;
    private float _rotatedTime;

    [SerializeField] private LayerMask _cameraMask;
    [SerializeField] private float _offsetFromBounds = 2f;

    [SerializeField, Header("Beam")] Transform _beamTransform;
    [SerializeField] float _beamSpeed = 1f;
    Vector3 _beamHitPoint;
    bool _beamHit = false;

    [SerializeField, Header("Aiming laser")] private LineRenderer _lineRenderer;
    [SerializeField] Gradient _probeBeamColor;
    [SerializeField] private Material _beamMaterial;
    [SerializeField] private Material _defaultMaterial;

    private bool _active = true;
    void Start()
    {
        _target = GameObject.FindGameObjectWithTag("Player").transform;
        _rotatedTime = 0f;
        _active = true;

        StartCoroutine(RepeatingRoutine());
    }

    void Update()
    {

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

    IEnumerator RepeatingRoutine()
    {
        this.transform.position = GetStartPosition();

        Vector3 startPosition = Helper.Cam.GetRandomPositionInRect(0.1f, 0.9f, 0.9f, 0.5f);
        var targetRotation = Quaternion.LookRotation(Vector3.forward, startPosition - this.transform.position);
        this.transform.rotation = targetRotation;
        yield return StartCoroutine(MoveToRoutine(startPosition, _speed));

        while (_active)
        {
            yield return StartCoroutine(WaitAndLookRoutine());
            yield return new WaitForSeconds(0.3f);
            yield return StartCoroutine(ShootBeamRoutine());
            yield return new WaitForSeconds(0.3f);

            var nextPosition = Helper.Cam.GetRandomPositionInRect(0.1f, 0.9f, 0.9f, 0.5f);
            yield return StartCoroutine(MoveToRoutine(nextPosition, _speed));
        }
    }
    IEnumerator WaitAndLookRoutine()
    {
        float beamMaxLength = Helper.Cam.DiagonalLength();
        _lineRenderer.enabled = true;
        _beamTransform.gameObject.SetActive(false);
        SetBeamLineToProbeColor();

        while (_rotatedTime < _rotateTime)
        {
            var direction = _target.position - transform.position;
            direction.z = 0f;
            var targetRotation = Quaternion.LookRotation(Vector3.forward, direction);
            this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, targetRotation, _rotateSpeed * Time.deltaTime);

            Vector2 startPosition = this.transform.position;

            //Vector2 endPosition;
            //var hit = Physics2D.Raycast(startPosition, direction, beamMaxLength, _cameraMask);
            //if (hit.collider != null) // hit the camera rect
            //{
            //    endPosition = hit.collider.transform.position;
            //}
            //else
            //{
            //    Vector2 direction2d = direction;
            //    endPosition = startPosition + direction2d.normalized * beamMaxLength;
            //}

            Vector2 direction2d = direction;
            //Vector2 endPosition = startPosition + direction2d.normalized * beamMaxLength;
            Vector2 endPosition = startPosition + new Vector2(transform.up.x, transform.up.y) * beamMaxLength;

            _lineRenderer.SetPositions(
                new Vector3[] {
                    new Vector3(startPosition.x, startPosition.y, 0),
                    new Vector3(endPosition.x, endPosition.y, 0) });

            _rotatedTime += Time.deltaTime;
            yield return null;
        }

    }

    IEnumerator ShootBeamRoutine()
    {
        _lineRenderer.enabled = false;
        _beamTransform.gameObject.SetActive(true);

        float beamMaxLength = Helper.Cam.DiagonalLength() + 1;
        Vector3 startPosition = this.transform.position; startPosition.z = 0f;
        Vector3 endPosition = startPosition + this.transform.up * beamMaxLength;
        Vector3 nextPosition = startPosition;

        _beamHit = false;
        // * Should put logic on the same frame as FixedUpdate
        while (true)
        {
            yield return new WaitForFixedUpdate();

            if (!_beamHit && Vector2.Distance(nextPosition, endPosition) > Mathf.Epsilon)
            {
                nextPosition = Vector3.MoveTowards(nextPosition, endPosition, Time.fixedDeltaTime * _beamSpeed);
                _beamTransform.localScale = new Vector3(_beamTransform.localScale.x, Vector2.Distance(startPosition, nextPosition), 1f);
            }
            else if (_beamHit)
            {
                _beamTransform.localScale = new Vector3(_beamTransform.localScale.x, Vector2.Distance(startPosition, _beamHitPoint), 1f);
                nextPosition = _beamHitPoint;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
    }

    private void OnTriggerStay2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {
            _beamHit = true;
            _beamHitPoint = collision.ClosestPoint(_beamTransform.position);
            Debug.Log("Enter Hit " + _beamHitPoint);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _beamHit = false;
            Debug.Log("Exit Hit ");
        }
    }

    private IEnumerator MoveToRoutine(Vector3 targetPos, float speed)
    {
        while (Vector2.Distance(transform.position, targetPos) > Mathf.Epsilon)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
            yield return null;
        }
    }
    private IEnumerator RotateToRoutine(Vector3 targetPos, float rotateSpeed)
    {
        var directionToTarget = (targetPos - transform.position);
        var targetRotation = Quaternion.LookRotation(Vector3.forward, directionToTarget);
        while (transform.rotation != targetRotation)
        {
            yield return null;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
        }
    }
    IEnumerator ShootLaserRoutineWithLineRenderer()
    {
        while (_rotatedTime < _rotateTime)
        {
            var direction = _target.position - transform.position;
            direction.z = 0f;
            var targetRotation = Quaternion.LookRotation(Vector3.forward, direction);
            this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, targetRotation, _rotateSpeed * Time.deltaTime);

            _rotatedTime += Time.deltaTime;
            yield return null;
        }
    }

    void SetBeamLineToProbeColor()
    {
        _lineRenderer.colorGradient = _probeBeamColor;
        _lineRenderer.material = _defaultMaterial;
    }

    void SetBeamLineToActiveColor()
    {
        _lineRenderer.startColor = Color.white;
        _lineRenderer.endColor = Color.white;
        _lineRenderer.material = _beamMaterial;
    }
}
