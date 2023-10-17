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
    [SerializeField] string _tag_EnemyBullet;
    [SerializeField] string _tag_PlayerBullet;
    public string Tag_EnemyBullet => _tag_EnemyBullet;
    public string Tag_PlayerBullet => _tag_PlayerBullet;
    //--------------------------------------------

    //--------------Parents-----------------------
    [SerializeField] Transform _vfxParent;
    [SerializeField] Transform _bulletParent;
    public Transform VFXParent => _vfxParent;
    public Transform BulletParent => _bulletParent;
    //--------------------------------------------

    //--------------Prefabs-----------------------
    [SerializeField] GameObject _damagePopupPrefab;
    public GameObject DamagePopPrefab => _damagePopupPrefab;
    //--------------------------------------------
};
