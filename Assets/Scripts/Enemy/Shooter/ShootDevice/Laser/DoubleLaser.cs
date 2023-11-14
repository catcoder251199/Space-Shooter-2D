using UnityEngine;
using UnityEngine.Events;

public class DoubleLaser : MonoBehaviour
{
    public UnityEvent OnDoubleLaserFinished; 
    [SerializeField, Header("First Laser")] Laser _firstLaser;
    [SerializeField, Header("Second Laser")] Laser _secondLaser;
    [SerializeField] bool _isShooting = true;

    private int _launchedCount = 0;

    private void Start()
    {
    }
    public void Launch()
    {
        if (IsShooting())
            return;
        Debug.Log("Start Double");


        _isShooting = true;
        _launchedCount = 2;

        _firstLaser.transform.localRotation = Quaternion.identity;
        _firstLaser.transform.localPosition = Vector3.zero;
        _firstLaser.Launch();

        _secondLaser.transform.localRotation = Quaternion.identity;
        _secondLaser.transform.localPosition = Vector3.zero;
        _secondLaser.Launch();
    }

    // * subscribe to SingleLaserFinishedEvent
    public void OnOneLaserLaunchFinished()
    {
        _launchedCount -= 1;
        if (_launchedCount <= 0)
        {
            OnDoubleLaserShootingFinished();
        }
    }
    private void OnDoubleLaserShootingFinished()
    {
        Debug.Log("Finish Double");
        _isShooting = false;
        OnDoubleLaserFinished?.Invoke();
    }

    public bool IsShooting() => _isShooting && _launchedCount > 0;
}
