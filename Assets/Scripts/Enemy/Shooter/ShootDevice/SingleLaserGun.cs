using System;
using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class SingleLaserGun : LaserGunBase
{
    public event Action OnAttackFinishedEvent;

    public enum Mode
    {
        None,
        Probe,
        Shoot
    }
    private Mode _currentMode = Mode.None;

    [SerializeField] Transform _spawnLocation;
    [SerializeField] int _damage = 1;

    [SerializeField, Header("Laser")] private LineRenderer _probingLineRenderer;
    [SerializeField] Laser _laser;
    [SerializeField] Gradient _probeBeamColor;

    [SerializeField] float _blinkTime = 1f;
    [SerializeField, Range(4, 18)] int _blinkCount = 4;
    public int Damage => _damage;

    private bool _isShooting = false;

    private void Start()
    {
        InitProbingLine();
    }

    private void InitProbingLine()
    {
        _probingLineRenderer.colorGradient = _probeBeamColor;
        _probingLineRenderer.startWidth = 0.05f;
        _probingLineRenderer.endWidth = 0.05f;
        float beamLength = 20f; // Helper.Cam.DiagonalLength();
        Vector3 startPosition = _spawnLocation.localPosition; startPosition.z = 0f;
        Vector3 endPosition = startPosition + Vector3.up * beamLength;
        _probingLineRenderer.SetPositions(new Vector3[] { startPosition, endPosition });
        _probingLineRenderer.enabled = false;
    }

    public void SetProbingModeEnabled(bool enabled)
    {
        _probingLineRenderer.enabled = enabled;
        _probingLineRenderer.colorGradient = _probeBeamColor;
    }
    public override void Activate()
    {
        if (_currentMode == Mode.Shoot && !_isShooting)
        {
            _isShooting = true;
            StartCoroutine(ShootLaserRoutine());
        }
    }

    private IEnumerator ShootLaserRoutine()
    {
        yield return StartCoroutine(ProbingRoutine());
        _probingLineRenderer.enabled = false;
        _laser.Launch();
    }

    public bool IsInMode(Mode mode) => _currentMode == mode;

    public void SwitchMode(Mode mode)
    {
        if (_currentMode == mode)
            return;

        switch (mode)
        {
            case Mode.None:
                _probingLineRenderer.enabled = false;
                _laser.gameObject.SetActive(false);
                break;
            case Mode.Probe:
                _probingLineRenderer.enabled = true;
                _laser.gameObject.SetActive(false);
                break;
            case Mode.Shoot:
                _isShooting = false;
                _probingLineRenderer.enabled = true;
                _laser.gameObject.SetActive(true);
                _laser.transform.localScale = new Vector3(0.1f, 0.01f, 1f);
                break;
        }

        _currentMode = mode;
    }

    private void Update()
    {
        switch (_currentMode)
        {
            case Mode.None:
                break;
            case Mode.Probe:
                break;
            case Mode.Shoot:
                break;
        }
    }
    private IEnumerator ProbingRoutine()
    {
        yield return StartCoroutine(BlinkProbingLine(true));
        _probingLineRenderer.enabled = false;
    }

    IEnumerator BlinkProbingLine(bool firstEnabled)
    {
        //* blinedCount increases by 1 whenever it is switch off (from on) or on (from off)
        _probingLineRenderer.enabled = firstEnabled;
        int blinkedCount = 0;
        float timeBetweenOnAndOff = _blinkTime / _blinkCount;
        while (blinkedCount < _blinkCount)
        {
            yield return new WaitForSeconds(timeBetweenOnAndOff);
            _probingLineRenderer.enabled = !_probingLineRenderer.enabled;
            blinkedCount++;
        }
    }

    public void OnLaserBeamDisappered()
    {
        _isShooting = false;
        OnAttackFinishedEvent?.Invoke();
    }
}