using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [Header("Settings")]
    public GameObject minionPrefab;       // 丟出去的小怪 Prefab
    public float spawnInterval = 3f;      // 每幾秒丟一次
    public float launchForce = 10f;       // 拋射的力道（高度）

    private float nextSpawnTime;

    void Update()
    {
        // 確保遊戲沒暫停且 Boss 已經出現
        if (Time.time >= nextSpawnTime)
        {
            LaunchMinion();
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    void LaunchMinion()
    {
        if (PlayerManager.activePlayer == null || minionPrefab == null) return;

        // 1. 在 Boss 位置生成小怪
        GameObject minion = Instantiate(minionPrefab, transform.position, Quaternion.identity);

        // 2. 獲取拋物線腳本並給予目標位置
        ParabolicLaunch projectile = minion.AddComponent<ParabolicLaunch>();
        projectile.Setup(PlayerManager.activePlayer.transform, launchForce);
    }
}