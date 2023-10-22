using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FallingBackgroundAsteroid : MonoBehaviour
{
    [SerializeField] private Sprite[] _visualList;

    [SerializeField] float _fallingSpeedMin = 1;
    [SerializeField] float _fallingSpeedMax = 3;

    [SerializeField] float _rotateSpeedMin = 0;
    [SerializeField] float _rotateSpeedMax = 30;

    [SerializeField] float _scaleXYMin = 1;
    [SerializeField] float _scaleXYMax = 3;

    [SerializeField] Color _firstColor;
    [SerializeField] Color _secondColor;

    public float StartDelay = 0f;
    public float LimitTop = 9f;
    public float LimitBottom = -9f;
    public float LimitLeft = 5f;
    public float LimitRight = -5f;

    private float _fallingSpeed;
    private float _rotateSpeed;

    private SpriteRenderer _renderer;
    private bool _updateEnabled = false;

    void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        Reset();
        StartCoroutine(StartRoutine());
    }

    private IEnumerator StartRoutine()
    {
        yield return new WaitForSeconds(StartDelay);
        _updateEnabled = true;
    }

    void Update()
    {
        if (!_updateEnabled)
            return;

        if (transform.position.y < LimitBottom)
        {
            Reset();
        }
        transform.position += -Vector3.up * _fallingSpeed * Time.deltaTime;
        transform.Rotate(transform.forward, _rotateSpeed * Time.deltaTime, Space.Self);
    }

    private void Reset()
    {
        _fallingSpeed = Random.Range(_fallingSpeedMin, _fallingSpeedMax);
        _rotateSpeed = Random.Range(_rotateSpeedMin, _rotateSpeedMax);

        float scaleXY = Random.Range(_scaleXYMin, _scaleXYMax);
        transform.localScale = new Vector3(scaleXY, scaleXY, 1);


        if (_renderer != null)
        {
            _renderer.sprite = _visualList[Random.Range(0, _visualList.Length)];
            _renderer.color = Color.Lerp(_firstColor, _secondColor, Random.Range(0f, 1f));
        }

        float x = Random.Range(LimitLeft, LimitRight);
        transform.position = new Vector3(x, LimitTop, 0);
    }
}
