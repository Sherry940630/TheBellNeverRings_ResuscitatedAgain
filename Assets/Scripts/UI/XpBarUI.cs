using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class XpBarUI : MonoBehaviour
{
    public Image xpFillImage;
    public Text levelText;

    public PlayerExperience playerManager;

    void Update()
    {
        if (playerManager == null) return;

        levelText.text = "LV." + playerManager.playerLevel;

        xpFillImage.fillAmount =
            (float)playerManager.experience / playerManager.experienceCap;
    }
}
