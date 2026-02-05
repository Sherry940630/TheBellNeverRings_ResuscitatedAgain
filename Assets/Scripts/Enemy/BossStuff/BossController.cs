using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [Header("Settings")]
    public GameObject minionPrefab; 
    public float spawnInterval = 3f; //每幾秒丟一次
    public float launchForce = 10f; 
    public BossHealthBar bossHealthBar;

    private float nextSpawnTime;
    private EnemyStat stat;

    private void Start()
    {
        stat = GetComponent<EnemyStat>();
        bossHealthBar.Bind(stat);
        stat.OnHealthChanged += bossHealthBar.UpdateHealth;
    }

    private void Update()
    {
        // 確保遊戲沒暫停且 Boss 已經出現
        if (Time.time >= nextSpawnTime)
        {
            LaunchMinion();
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    private void LaunchMinion()
    {
        if (PlayerManager.activePlayer == null || minionPrefab == null) return;

        // 1. 在 Boss 位置生成小怪
        GameObject minion = Instantiate(minionPrefab, transform.position, Quaternion.identity);

        // 2. 獲取拋物線腳本並給予目標位置
        ParabolicLaunch projectile = minion.AddComponent<ParabolicLaunch>();
        projectile.Setup(PlayerManager.activePlayer.transform, launchForce);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (PlayerManager.Instance == null) return;

        PlayerManager.Instance.TryApplyDamage(
            other.gameObject,
            stat.enemyData.Damage,
            out float newHP
        );
    }
}