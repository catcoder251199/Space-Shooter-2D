using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Wave", menuName = "ScriptableObjects/Enemy Wave")]
public class WaveSO : ScriptableObject
{
    [Serializable]
    public class SpawnedObject 
    {
        public GameObject prefab;
        public int count;
        public float nextSpawnDelay = 0f;
        public float spawnNextGroupDelay = 0f;
    }
    [SerializeField] private SpawnedObject[] _spawnedObjectsList;

    public SpawnedObject[] SpawnedObjectsList => _spawnedObjectsList;
}
