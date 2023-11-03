using System.Collections;
using System;
using UnityEngine;

public class LaserGunDevice : LaserGunBase
{

    [SerializeField, Header("Sight line")] private LineRenderer _sightLineRenderer;
    [SerializeField] private Color _sightLineColor;
    [SerializeField] private float _sightLineWidth;
    [SerializeField] private float _maxSightLineLength = 20;
    [SerializeField] private bool _sightLineEnabledOnStart = false;
    [SerializeField] private Transform _sightLineStartLocation;
    [SerializeField] private Blink2Colors _sightLineBlinkAnimator;

    [SerializeField, Header("Laser Beam")] private Laser _laserBeam;
    [SerializeField] private bool _beamEnabledOnStart = false;
    [SerializeField] private float _beamMaxLength = 20;

    private void Awake()
    {
        if (_sightLineRenderer == null)
            _sightLineRenderer = GetComponent<LineRenderer>();
        if (_laserBeam == null)
            _laserBeam = GetComponentInChildren<Laser>();
    }

    private void Start()
    {
        // M* ust call initializing methods in start() because it needs their child (laser) call awake() first
        InitializeSightLine(_sightLineEnabledOnStart, _maxSightLineLength);
        InitializeLaser(_beamEnabledOnStart, _beamMaxLength);
    }

    private void OnEnable()
    {
        if (_sightLineBlinkAnimator != null)
            _sightLineBlinkAnimator.OnColorChanged += SetSightLineColor;
        if (_laserBeam != null)
            _laserBeam.OnLaserBeamStop.AddListener(OnLaserBeamStopped);
    }

    private void OnDisable()
    {
        if (_sightLineBlinkAnimator != null)
            _sightLineBlinkAnimator.OnColorChanged -= SetSightLineColor;
        if (_laserBeam != null)
            _laserBeam.OnLaserBeamStop.RemoveListener(OnLaserBeamStopped);
    }

    //---Sight Line Section---
    private void SetSightLineColor(Color color) => _sightLineRenderer.startColor = _sightLineRenderer.endColor = color;
    private void SetSightLineWidth(float width) => _sightLineRenderer.startWidth = _sightLineRenderer.endWidth = width;
    private void SetSightLineLength(float len)
    {
        Vector3 startPosition = _sightLineStartLocation.localPosition;
        Vector3 endPosition = startPosition + Vector3.up * len;
        _sightLineRenderer.SetPositions(new Vector3[] { startPosition, endPosition });
    }
    public bool IsSightLineEnabled() => _sightLineRenderer.enabled;
    private void InitializeSightLine(bool firstEnabled, float firstLen)
    {
        SetSightLineColor(_sightLineColor);
        SetSightLineWidth(_sightLineWidth);
        SetSightLineLength(firstLen);
        SetSightLineEnabled(firstEnabled);

        if (_sightLineBlinkAnimator != null)
        {
            _sightLineBlinkAnimator.interpolate = false; // transition to new color happens instantly
            _sightLineBlinkAnimator.useCustomRenderer = true;
            _sightLineBlinkAnimator.changeBlinkSpeedOnCountDown = true;
        }
    }
    public override void SetSightLineEnabled(bool enabled) => _sightLineRenderer.enabled = enabled;


    //---Laser Section---
    private void InitializeLaser(bool firstEnabled, float firstLen)
    {
        //SetLaserBeamEnabled(firstEnabled);
        _laserBeam.SetMaxLength(_beamMaxLength);
        _laserBeam.SetCurrentLength(firstLen);
    }
    private IEnumerator LaunchLaserBeamRoutine(float delay, bool blinkOnDelay)
    {
        if (IsSightLineEnabled() && blinkOnDelay)
        {
            _sightLineBlinkAnimator.SetTimer(delay);
            _sightLineBlinkAnimator.StartAnimationWithTimer();
        }
        yield return new WaitForSeconds(delay);

        SetSightLineColor(_sightLineColor); // switch sight line's color back to original color
        SetSightLineEnabled(false); // turn off the sight line
        _laserBeam.Launch(); // launch the laser beam
    }
    private void OnLaserBeamStopped()
    {
        _onAttackFinished?.Invoke();
    }
    public override void ActivateLaserBeam(float delay, bool blinkOnDelay, Action onFinished)
    {
        if (!_laserBeam.IsAvailableToShoot())
            return;

        _onAttackFinished = onFinished;
        StartCoroutine(LaunchLaserBeamRoutine(delay, blinkOnDelay));
    }
}
