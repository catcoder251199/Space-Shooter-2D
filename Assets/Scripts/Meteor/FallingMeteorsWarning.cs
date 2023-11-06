using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class FallingMeteorsWarning : MonoBehaviour
{
    [SerializeField] FallingMeteors _meteors;
    [SerializeField] private LayerMask _layerMask;

    [SerializeField] private float _minScale = 0.5f;
    [SerializeField] private float _maxSCale = 1.5f;

    [SerializeField] private GameObject _triangle;
    [SerializeField] private GameObject _circle;
    [SerializeField] private float _blinkDelay = 0.1f;

    private Coroutine _blinkRoutine;

    public FallingMeteors FallingMeteors
    {
        private get { return _meteors; }
        set { _meteors = value; }
    }

    private void Start()
    {
        _blinkRoutine = StartCoroutine(BlinkRoutine());
    }

    void Update()
    {
        if (_meteors == null)
            return;

        // If meteors are near the camera enough
        if (Helper.Cam.IsPositionInWorldCamRect(_meteors.transform.position, 2f))
        {
            // No need the warning
            Destroy(this.gameObject);
        }

        // Meteors are being off the screen
        float maxDistanceToCamera = _meteors.GetOffsetFromBounds();
        float minDistanceToCamera = maxDistanceToCamera / 10f;
        Transform meteorsTransform = _meteors.transform;
        var hit = Physics2D.Raycast(meteorsTransform.position, meteorsTransform.up, maxDistanceToCamera, _layerMask);
        if (hit)
        {
            // The meteors are falling towards the camera
            this.transform.position = hit.point;

            float scaleXY = Mathf.Lerp(_maxSCale, _minScale, Mathf.InverseLerp(minDistanceToCamera, maxDistanceToCamera, hit.distance));
            this.transform.localScale = new Vector3(scaleXY, scaleXY, 1);
        }
    }

    private IEnumerator BlinkRoutine()
    {
        if (_triangle == null || _circle == null)
            yield break;
        var triangleRenderer = _triangle.GetComponent<SpriteRenderer>();
        var circleRenderer = _circle.GetComponent<SpriteRenderer>();

        var triangleColor1 = triangleRenderer.color;
        var circleColor1 = circleRenderer.color;
        var triangleColor2 = new Color(triangleColor1.r, triangleColor1.g, triangleColor1.b, triangleColor1.a * 0.5f);
        var circleColor2 = new Color(circleColor1.r, circleColor1.g, circleColor1.b, circleColor1.a * 0.5f);

        while (true)
        {
            yield return new WaitForSeconds(_blinkDelay);
            triangleRenderer.color = triangleColor1;
            circleRenderer.color = circleColor1;
            yield return new WaitForSeconds(_blinkDelay);
            triangleRenderer.color = triangleColor2;
            circleRenderer.color = circleColor2;
        }
    }

    private void Deactivate()
    {

    }
}
