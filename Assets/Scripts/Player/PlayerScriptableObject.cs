using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerData", menuName = "ScriptableObjects/Player")]
public class PlayerScriptableObject : ScriptableObject
{
    [Header("Basic Info")]
    public string playerName;
    public Sprite playerSprite;      
    public RuntimeAnimatorController animatorController;

    [Header("Stats")]
    public float maxHealth = 100f;
    public float moveSpeed = 5f;

    [Header("Starting Weapon")]
    public WeaponScriptableObject startingWeapon;

    [Header("Manual Skill")]
    public SkillBehavior manualSkill;
}
