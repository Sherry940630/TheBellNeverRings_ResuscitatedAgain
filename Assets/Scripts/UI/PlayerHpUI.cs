using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHPUI : MonoBehaviour
{
    public Image hpFillImage;
    public Image skillCdFillImage; 

    public GameObject targetPlayer;

    private float skillMaxCd = 3f; //temp!!!!!!!

    void Update()
    {
        if (PlayerManager.Instance == null) return;

        if (!PlayerManager.Instance.TryGetRuntimeData(targetPlayer, out var data))
            return;

        hpFillImage.fillAmount =
            data.currentHealth / data.baseData.maxHealth;

        float cooldownRatio = Mathf.Clamp01(
            data.skillCooldownTimer / skillMaxCd);
        skillCdFillImage.fillAmount = 1- cooldownRatio;

        //Debug.Log("Updating fill amount to " + data.currentHealth / data.baseData.maxHealth);
    }
}
