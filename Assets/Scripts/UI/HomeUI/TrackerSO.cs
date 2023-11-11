using UnityEngine;
using UnityEngine.Jobs;

[CreateAssetMenu(fileName = "New Tracker", menuName = "ScriptableObjects/Tracker")]
public class TrackerSO : ScriptableObject
{
    public int selectedSpaceShip = 0;
    public SpaceShipSO selectedSpaceShipSO; 
}