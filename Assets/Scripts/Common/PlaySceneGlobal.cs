using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlaySceneGlobal : MonoBehaviour
{
    public static PlaySceneGlobal Instance { get; private set; }
    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    //--------------Tags-------------------------
    [SerializeField] string _tagPlayer = "Player";
    [SerializeField] string _tagEnemyBullet = "EnemyBullet";
    [SerializeField] string _tagPlayerBullet = "PlayerBullet";
    public string Tag_EnemyBullet => _tagEnemyBullet;
    public string Tag_PlayerBullet => _tagPlayerBullet;
    public string Tag_Player => _tagPlayer;


    //--------------------------------------------

    //--------------Parents-----------------------
    [SerializeField] Transform _vfxParent;
    [SerializeField] Transform _bulletParent;
    [SerializeField] Transform _spawnedObjectParent;

    public Transform VFXParent => _vfxParent;
    public Transform BulletParent => _bulletParent;
    public Transform SpawnedObjectParent => _spawnedObjectParent;

    //--------------------------------------------

    //--------------Prefabs-----------------------
    [SerializeField] GameObject _damagePopupPrefab;
    public GameObject DamagePopPrefab => _damagePopupPrefab;
    //--------------------------------------------
};
