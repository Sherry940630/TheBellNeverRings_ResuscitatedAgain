using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnRate = 1.5f;
    public float xRange = 8f;
    private float spawnTimer;

    void Update()
    {
        if (Time.timeScale == 0f) return;

        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnRate)
        {
            Spawn();
            spawnTimer = 0f;
        }
    }

    void Spawn()
    {
        Vector2 spawnPos = new Vector2(Random.Range(-xRange, xRange), 5f);
        Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
    }

}
