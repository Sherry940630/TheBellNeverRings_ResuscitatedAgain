using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MinionSpawnConfig
{
    public GameObject minionPrefab;
    public float spawnInterval = 3f;
    public float launchForce = 10f;
}