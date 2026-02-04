using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "ScriptableObjects/Skill Data")]
public class SkillData : ScriptableObject
{
    public float cooldown;
    public float damage;
}
