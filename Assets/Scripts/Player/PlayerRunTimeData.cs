using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/*
  When the class¡¦s data is serialized, 
  it is saved, loaded, and displayed in the Inspector.
*/
[System.Serializable]
public class PlayerRunTimeData
{
    [Header("Player Base Stats")]
    public PlayerScriptableObject baseData;
    public float currentHealth;
    public float skillCooldownTimer;
    public bool isActive;

    [Header("State Flags")]
    public bool isInvincible;

    [Header("Upgrades")]
    public List<UpgradeSO> ownedUpgrades = new List<UpgradeSO>();
    public Dictionary<UpgradeSO, int> upgradeLevels
        = new Dictionary<UpgradeSO, int>();

    public PlayerRunTimeData(PlayerScriptableObject data)
    {
        isInvincible = false;

        baseData = data;
        currentHealth = data.maxHealth;
        isActive = false;
        skillCooldownTimer = 3f;
    }
}
