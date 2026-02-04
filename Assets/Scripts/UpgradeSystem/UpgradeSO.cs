using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Upgrade")]

public class UpgradeSO : ScriptableObject
{
    [Header("Player Restriction")]
    public List<string> allowedPlayerNames;

    [Header("Upgrade Info")]
    public string upgradeName;
    public Sprite icon;
    public UpgradeType type;

    public WeaponScriptableObject weaponToGrant;

    public virtual bool CanApply(GameObject player)
    {
        if (!PlayerManager.Instance.TryGetRuntimeData(player, out PlayerRunTimeData data))
            return false;

        if (allowedPlayerNames != null && allowedPlayerNames.Count > 0)
        {
            if (!allowedPlayerNames.Contains(player.name))
                return false;
        }

        bool hasUpgrade = data.upgradeLevels.ContainsKey(this);

        // 新武器：可以出現第一次
        if (type == UpgradeType.NewWeapon && !hasUpgrade)
            return true;

        // 升級：已經拿過才能再出現
        if (type == UpgradeType.WeaponUpgrade && hasUpgrade)
            return true;

        return false;
    }


    public virtual void Apply(GameObject player)
    {
        if (!PlayerManager.Instance.TryGetRuntimeData(player, out PlayerRunTimeData data))
            return;

        if (!data.ownedUpgrades.Contains(this))
            data.ownedUpgrades.Add(this);

        if (!data.upgradeLevels.ContainsKey(this))
            data.upgradeLevels[this] = 1;
        else
            data.upgradeLevels[this]++;

        if (type == UpgradeType.NewWeapon && weaponToGrant != null)
        {
            Instantiate(
                weaponToGrant.WeaponControllerPrefab,
                player.transform
            );
        }
        else
        {
            Debug.LogWarning($"UpgradeSO {name} has no weapon prefab assigned!");
        }
    }

    public virtual GameObject FindTargetPlayer()
    {
        if (PlayerManager.Instance == null) return null;

        foreach (var player in PlayerManager.Instance.players)
        {
            if (CanApply(player))
                return player;
        }
        return null;
    }

    public virtual PlayerScriptableObject GetTargetPlayerData()
    {
        GameObject target = FindTargetPlayer();
        if (target == null) return null;

        if (PlayerManager.Instance.TryGetRuntimeData(target, out PlayerRunTimeData data))
            return data.baseData;

        return null;
    }

}
