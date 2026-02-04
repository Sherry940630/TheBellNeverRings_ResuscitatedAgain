using UnityEngine;
using System;

public class EnemyStat : MonoBehaviour
{
    public static event Action<EnemyStat> OnEnemyKilled;
    public EnemyScriptableObject enemyData;
    public GameObject killEffectPrefab; //bloodthirst斬殺特效

    private EnemyHealthBar healthBar;
    private float currentMoveSpeed;
    private float currentHealth;
    private float currentDamage;

    private void Awake()
    {
        currentMoveSpeed = enemyData.MoveSpeed;
        currentHealth = enemyData.MaxHealth;
        currentDamage = enemyData.Damage;

        healthBar = GetComponentInChildren<EnemyHealthBar>();
        healthBar.SetHealth(currentHealth, enemyData.MaxHealth);
    }

    public void TakeDamage(float dmg)
    {
        TakeDamage(dmg, "");
    }

    public void TakeDamage(float dmg, string sourceTag)
    {
        currentHealth -= dmg;
        healthBar.SetHealth(currentHealth, enemyData.MaxHealth);

        if (currentHealth <= 0)
        {
            //只有在血量歸零的那一擊來源是bloodthirst時才觸發
            if (sourceTag == "Bloodthirst")
            {
                SpawnKillEffect();
                Bloodthirst blood = FindObjectOfType<Bloodthirst>();
                if (blood != null)
                {
                    blood.ResetCooldown();
                }
                else
                {
                    Debug.LogWarning("Bloodthirst instance not found; cannot reset cooldown.");
                }
            }
            Kill();
        }
    }

    public void Kill()
    {
        //Notify all listeners that this enemy has been killed
        OnEnemyKilled?.Invoke(this);

        Destroy(gameObject);
    }

    private void SpawnKillEffect()
    {
        if (killEffectPrefab != null)
        {
            float effectDuration = 0.5f;
            GameObject effect = Instantiate(killEffectPrefab, transform.position, Quaternion.identity);
            Destroy(effect, effectDuration);
        }
    }
}
