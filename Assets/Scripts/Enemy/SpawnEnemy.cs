using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnRate = 1.5f;
    public float xRange = 8f;
    void Start()
    {
        InvokeRepeating("Enemy", 1f, spawnRate);
    }
    void Enemy()
    {
        Vector2 spawnPos = new Vector2(Random.Range(-xRange, xRange), 5f);
        Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
    }
}
