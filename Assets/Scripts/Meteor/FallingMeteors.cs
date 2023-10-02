using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FallingMeteors : MonoBehaviour
{
    [SerializeField] private int _size;
    [SerializeField] private float _speed = 1f;
    [SerializeField] private float _gap = 1f;
    [SerializeField] private float _offsetFromBounds = 2f;
    [SerializeField] private GameObject[] _meteorList;

    [SerializeField] private GameObject _warningPrefab;
    private FallingMeteorsWarning _warning;

    bool _isInsideScreen = false;

    public float GetOffsetFromBounds() => _offsetFromBounds;

    private void Awake()
    {
        Initialize();
        StartToWarn();
    }

    private void FixedUpdate()
    {
        if (!_isInsideScreen && IsOnScreen())
        {
            _isInsideScreen = true;
            OnEnterScreen();
        }
        else if (_isInsideScreen && IsOffScreen())
        {
            _isInsideScreen = false;
            OnExitScreen();
        }

        MoveForward();
    }

    void OnEnterScreen()
    {

    }
    void OnExitScreen()
    {
        Destroy(gameObject, 2f);
    }

    private void MoveForward()
    {
        this.transform.Translate(Vector3.up * _speed * Time.fixedDeltaTime);
    }

    private void StartToWarn()
    {
        _warning = Instantiate(_warningPrefab).GetComponent<FallingMeteorsWarning>();
        _warning.FallingMeteors = this;
    }

    private void Initialize()
    {
        _size = Mathf.Max(1, _size); //* Must have at least 3 meteors

        //float gap = Random.Range(0.5f, Mathf.Min(0.5f,_gap));
        float gap = _gap;

        float startX = 0f;
        Func<float, int, float> GetBasePosX = (startX, index) => startX + gap * index;
        float delta = Mathf.Abs(GetBasePosX(startX, _size - 1) - GetBasePosX(startX, 0)); // distance between starting point and end point
        for (int i = 0; i < _size; i++)
        {
            float positionX = GetBasePosX(startX, i) - delta / 2;
            float positionY = -Random.Range(0f, 1f);
            Instantiate(_meteorList[Random.Range(0, _meteorList.Length - 1)], 
                new Vector3(positionX, positionY, 0f), Quaternion.identity, this.transform);
        }
        //----------------
        InitializeBeginPosition();
    }

    private void InitializeBeginPosition()
    {
        // Place groups on randomized position out sides of the screen
        // And rotate it to move foward in a randomized direciton
        //var sidesList = new Helper.Cam.Side[] { Helper.Cam.Side.Left, Helper.Cam.Side.Right, Helper.Cam.Side.Top };
        var sidesList = new Helper.Cam.Side[] { Helper.Cam.Side.Top };
        var spanwedSide = sidesList[Random.Range(0, sidesList.Length - 1)];

        if (spanwedSide == Helper.Cam.Side.Left)
        {
            Vector3 startPositionA = Helper.Cam.GetLeftSideRandomPos(_offsetFromBounds, 0, 1f, 1f); // Spawn the meteors at startPosition. Lets call it A
            this.transform.position = startPositionA;

            // Take Q as a point on the right edge but having same world y value as startPosition
            // Take P as a point on the right edge but having same world y value as bottom camera edge
            // Take B as a middle point between PQ on the right edge.
            // in the direction of decreasing y value, we have Q,B, P on the the same vertical right edge

            // Normalize the world y value. Then divide the derived value by 2 to get normalized y value of the middle point B and convert it back into world y.
            //Vector3 endPositionB = Helper.Cam.GetRightSidePos(Helper.Cam.GetVerticalEdge01Pos(startPositionA.y) / 2f);

            // But we may not necessarily to choose B as middle Point
            // This line below is another way to choose B at which BQ = 3/4 PQ.
            Vector3 endPositionB = Helper.Cam.GetRightSidePos(Helper.Cam.GetVerticalEdge01Pos(startPositionA.y) * 3f / 4); 

            // Take C as a middle point on the Bottom Camera Edge.
            Vector3 endPositionC = Helper.Cam.GetBottomSidePos(0.5f);

         
            Vector3 vecAB = endPositionB - startPositionA;
            Vector3 vecAC = endPositionC - startPositionA;
            float signedAngle = Vector3.SignedAngle(vecAB, vecAC, Vector3.forward);

            // MoveDirection is the direction which the meteors is orientated. This direciton forms with vector AB an angle theta.
            // Randomize Theta from 0f to singedAngle.
            float thetaAngle = Random.Range(0f, signedAngle);
            Vector2 moveDirection = Quaternion.Euler(0, 0, thetaAngle) * vecAB;
            this.transform.up = moveDirection;
        }
        else if (spanwedSide == Helper.Cam.Side.Right)
        {
            Vector3 startPositionA = Helper.Cam.GetRightSideRandomPos(_offsetFromBounds, 0, 1f, 1f);
            this.transform.position = startPositionA;

            Vector3 endPositionB = Helper.Cam.GetLeftSidePos(Helper.Cam.GetVerticalEdge01Pos(startPositionA.y) * 3f / 4);
            Vector3 endPositionC = Helper.Cam.GetBottomSidePos(0.5f);
            Vector3 vecAB = endPositionB - startPositionA;
            Vector3 vecAC = endPositionC - startPositionA;
            float signedAngle = Vector3.SignedAngle(vecAB, vecAC, Vector3.forward);
            float thetaAngle = Random.Range(0f, signedAngle);
            Vector2 moveDirection = Quaternion.Euler(0, 0, thetaAngle) * vecAB;
            this.transform.up = moveDirection;
        }
        else if (spanwedSide == Helper.Cam.Side.Top)
        {
            Vector3 startPosition = Helper.Cam.GetTopSideRandomPos(_offsetFromBounds, 0, 0f, 1f);
            this.transform.position = startPosition;
            Vector3 endPosition = Helper.Cam.GetBottomSideRandomPos(0f, 0f, 0.2f, 0.8f);
            Vector2 moveDirection = endPosition - startPosition;
            this.transform.up = moveDirection;
        }
        _isInsideScreen = false;
    }

    public bool IsOffScreen()
    {
        return Helper.Cam.IsPositionOutWorldCamRect(this.transform.position, 0);
    }

    public bool IsOnScreen()
    {
        return Helper.Cam.IsPositionInWorldCamRect(this.transform.position, 0);
    }
}
