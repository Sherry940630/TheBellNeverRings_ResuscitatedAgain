using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance;

    public List<UpgradeSO> allUpgrades;
    public GameObject upgradeUIPanel;
    public GameObject pausePanel;
    public UpgradeButton[] buttons;

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        PlayerExperience.OnLevelUp += HandleLevelUp;
    }

    private void OnDisable()
    {
        PlayerExperience.OnLevelUp -= HandleLevelUp;
    }

    private void HandleLevelUp(int level)
    {
        Time.timeScale = 0f;
        upgradeUIPanel.SetActive(true);
        pausePanel.SetActive(true);

        var cg = upgradeUIPanel.GetComponent<CanvasGroup>();
        var cg2 = pausePanel.GetComponent<CanvasGroup>();
        if (cg != null && cg2 != null)
        {
            cg.interactable = true;
            cg.blocksRaycasts = true;
            cg2.interactable = true;
            cg2.blocksRaycasts = true;
        }

        var validOptions = allUpgrades
         .Select(u => new
         {
             upgrade = u,
             target = u.FindTargetPlayer()
         })
         .Where(x => x.target != null)
         .OrderBy(_ => Random.value)
         .Take(3)
         .ToList();

        foreach (var btn in buttons)
        {
            btn.gameObject.SetActive(false);
            btn.Clear();   
        }

        for (int i = 0; i < validOptions.Count && i < buttons.Length; i++)
        {
            buttons[i].Setup
            (
                validOptions[i].upgrade,
                validOptions[i].target
            );
        }
    }


    public void ChooseUpgrade(UpgradeSO upgrade, GameObject target)
    {
        Debug.Log($"Chosen upgrade: {upgrade.upgradeName}, applied to {target.name}");
        upgrade.Apply(target);

        upgradeUIPanel.SetActive(false);
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }
}
