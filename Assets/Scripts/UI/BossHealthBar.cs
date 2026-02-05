using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    [SerializeField] private Image hpFillImage;

    private float maxHealth;

    public void Bind(EnemyStat stat)
    {
        maxHealth = stat.enemyData.MaxHealth;
        hpFillImage.fillAmount = 1f;
    }

    public void UpdateHealth(float current)
    {
        hpFillImage.fillAmount = Mathf.Clamp01(current / maxHealth);
    }
}
