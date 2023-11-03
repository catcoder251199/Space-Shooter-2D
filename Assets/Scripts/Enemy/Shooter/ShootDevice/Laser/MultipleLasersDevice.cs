using System;
using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class MultipleLasersDevice : LaserGunBase
{
    [SerializeField] LaserGunDevice[] _deviceList;
    bool _isShooting = false;
    int _launchedCount = 0;

    public override void ActivateLaserBeam(float delay, bool blinkOnDelay, Action onFinished) 
    {
        if (IsShooting())
            return;

        _onAttackFinished = onFinished;
        _launchedCount = _deviceList.Length;
        for (int i = 0; i < _deviceList.Length; i++)
            _deviceList[i].ActivateLaserBeam(delay, blinkOnDelay, OnOneLaserLaunchFinished);
    }

    public override void SetSightLineEnabled(bool enabled) 
    {
        for (int i = 0; i < _deviceList.Length; i++)
            _deviceList[i].SetSightLineEnabled(enabled);
    }
  
    public bool IsShooting() => _isShooting && _launchedCount > 0;
    private void OnOneLaserLaunchFinished()
    {
        _launchedCount -= 1;
        if (_launchedCount <= 0)
            OnAllLaserShootingFinished();
    }

    private void OnAllLaserShootingFinished()
    {
        _isShooting = false;
        TriggerOnAttackFinishedEvent();
    }
}