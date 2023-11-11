using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Space ship level up", menuName = "ScriptableObjects/SpaceShip/Level up")]
public class SpaceShipLevelUpSO : ScriptableObject
{
    [Serializable]
    public struct LevelUpInfo
    {
        public int hp;
        public int baseDamage;
        
    }
    public LevelUpInfo[] levelList;
  
    public int GetHpAtLevel(int level)
    {
        if (level < 0 || level >= levelList.Length) return -1;
        return levelList[level].hp;
    }

    public int GetBaseDamageAtLevel(int level)
    {
        if (level < 0 || level >= levelList.Length) return -1;
        return levelList[level].baseDamage;
    }
}