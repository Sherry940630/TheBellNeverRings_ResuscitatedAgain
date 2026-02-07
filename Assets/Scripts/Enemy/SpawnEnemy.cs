using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnRate = 1.5f;
    public float minSpawnDistance = 5f;
    public float maxSpawnDistance = 10f;
    public int maxTryCount = 80; //防止無限迴圈

    private float spawnTimer;

    void Update()
    {
        if (Time.timeScale == 0f) return;
        if (PlayerManager.activePlayer == null) return;

        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnRate)
        {
            Spawn();
            spawnTimer = 0f;
        }
    }

    void Spawn()
    {
        for (int i = 0; i < maxTryCount; i++)
        {
            Vector2 spawnPos = GetRandomPositionAroundActivePlayer();

            if (IsPositionValid(spawnPos))
            {
                Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
                return;
            }
        }
        //如果連續失敗（理論上很少）
        Debug.LogWarning("Enemy spawn failed: no valid position found.");
    }

    Vector2 GetRandomPositionAroundActivePlayer()
    {
        Vector2 center = PlayerManager.activePlayer.transform.position;

        float angle = Random.Range(0f, Mathf.PI * 2f);
        float distance = Random.Range(minSpawnDistance, maxSpawnDistance);

        Vector2 offset = new Vector2(
            Mathf.Cos(angle),
            Mathf.Sin(angle)
        ) * distance;

        return center + offset;
    }

    bool IsPositionValid(Vector2 pos)
    {
        foreach (GameObject player in PlayerManager.Instance.players)
        {
            if (player == null) continue;

            float dist = Vector2.Distance(pos, player.transform.position);
            if (dist < minSpawnDistance)
                return false;
        }
        return true;
    }

}
