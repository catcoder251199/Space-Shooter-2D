using System;
using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class DoubleLaserGun : LaserGunBase
{
    [SerializeField] SingleLaserGun _firstGun;
    [SerializeField] SingleLaserGun _secondGun;
    bool _isShooting = false;
    int _launchedCount = 0;

    public override float GetDelayTime() => Mathf.Max(_firstGun.GetDelayTime(), _firstGun.GetDelayTime());

    private void Awake()
    {
        _firstGun.OnAttackFinishedEvent += OnOneLaserLaunchFinished;
        _secondGun.OnAttackFinishedEvent += OnOneLaserLaunchFinished;
    }

    public override void Activate()
    {
        if (_currentMode == Mode.Shoot && !IsShooting())
        {
            _isShooting = true;
            _launchedCount = 2;
            _firstGun.Activate();
            _secondGun.Activate();
        }
    }
    public bool IsShooting() => _isShooting && _launchedCount > 0;

    public override void SwitchMode(Mode mode)
    {
        if (_currentMode == mode)
            return;

        _firstGun.SwitchMode(mode);
        _secondGun.SwitchMode(mode);

        _currentMode = mode;
    }
    public void OnOneLaserLaunchFinished()
    {
        _launchedCount -= 1;
        if (_launchedCount <= 0)
        {
            OnDoubleLaserShootingFinished();
        }
    }

    public void OnDoubleLaserShootingFinished()
    {
        _isShooting = false;
        TriggerOnAttackFinishedEvent();
    }
}