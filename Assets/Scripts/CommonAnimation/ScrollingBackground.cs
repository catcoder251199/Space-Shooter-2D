using System.Collections;
using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    [SerializeField] private Vector2 _move;
   

    [SerializeField, Header("Child Asteroid")] private FallingBackgroundAsteroid _asteroidPrefab;
    [SerializeField] private int _asteroidCount = 1;
    [SerializeField] private float _topLimit = 9f;
    [SerializeField] private float _bottomLimit = -9f;
    [SerializeField] private float _leftLimit = 5f;
    [SerializeField] private float _rightLimit = -5f;

    private Material _material;
    private Vector2 _offset;
    private void Awake()
    {
        _material = GetComponent<SpriteRenderer>().material;
        float timeDelay = 0f;
        for (int i = 0; i < _asteroidCount; i++)
        {
            var asteroid = Instantiate(_asteroidPrefab, transform.position, Quaternion.identity, transform);
            asteroid.LimitTop = _topLimit;
            asteroid.LimitBottom = _bottomLimit;
            asteroid.LimitLeft = _leftLimit;
            asteroid.LimitRight = _rightLimit;
            timeDelay += Random.Range(0, 2f);
            asteroid.StartDelay = timeDelay;
        }
    }

    private void Update()
    {
        if (_material != null)
        {
            _offset = -_move * Time.deltaTime;
            _material.mainTextureOffset += _offset;
        }
    }
}
