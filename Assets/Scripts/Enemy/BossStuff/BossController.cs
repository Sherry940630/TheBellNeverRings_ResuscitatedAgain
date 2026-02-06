using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [Header("Settings")]
    public BossHealthBar bossHealthBar;
    private EnemyStat stat;

    private void Start()
    {
        stat = GetComponent<EnemyStat>();
        bossHealthBar.Bind(stat);
        stat.OnHealthChanged += bossHealthBar.UpdateHealth;
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