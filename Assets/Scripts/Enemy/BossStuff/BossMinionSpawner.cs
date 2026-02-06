using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMinionSpawner : MonoBehaviour
{
    [Header("Minion Spawn Settings")]
    public List<MinionSpawnConfig> minionConfigs = new List<MinionSpawnConfig>();

    private List<float> nextSpawnTimes = new List<float>();

    private void Awake()
    {
        //為每一種 minion 初始化計時器
        foreach (var cfg in minionConfigs)
        {
            nextSpawnTimes.Add(Time.time + cfg.spawnInterval);
        }
    }

    private void Update()
    {
        if (PlayerManager.activePlayer == null) return;

        for (int i = 0; i < minionConfigs.Count; i++)
        {
            if (Time.time >= nextSpawnTimes[i])
            {
                SpawnMinion(minionConfigs[i]);
                nextSpawnTimes[i] = Time.time + minionConfigs[i].spawnInterval;
            }
        }
    }

    private void SpawnMinion(MinionSpawnConfig cfg)
    {
        if (cfg.minionPrefab == null) return;

        GameObject minion =
            Instantiate(cfg.minionPrefab, transform.position, Quaternion.identity);

        ParabolicLaunch launch =
            minion.AddComponent<ParabolicLaunch>();

        launch.Setup(PlayerManager.activePlayer.transform, cfg.launchForce);
    }
}
