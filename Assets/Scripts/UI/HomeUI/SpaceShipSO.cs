using UnityEngine;
using UnityEngine.Jobs;

[CreateAssetMenu(fileName = "New Space ship info", menuName = "ScriptableObjects/SpaceShip/BasicInfo")]
public class SpaceShipSO : ScriptableObject
{
    public Sprite spaceShipSprite;
    public Color trailColor;
    public PlayerBullet bulletPrefab;
    public SpaceShipLevelUpSO levelSO;
    [TextArea] public string description;
    public int level;
    //--
    public float baseCritRate = 0.2f;
    public float baseCritDamageModifier = 0.3f;
    public float baseSpeed = 8f;  

    public int GetCurrentLevel() { return Mathf.Clamp(level, 0, levelSO.levelList.Length - 1); }
    public int GetCurrentHp() { return levelSO.GetHpAtLevel(level); }
    public int GetCurrentBaseDamage() { return levelSO.GetBaseDamageAtLevel(level); }
    public int GetTotalLevel() { return levelSO.levelList.Length; }
    public int GetLastLevel() { return Mathf.Max(0, levelSO.levelList.Length - 1); }

}