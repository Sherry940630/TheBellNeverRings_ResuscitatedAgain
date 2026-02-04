using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static WeaponController;

[CreateAssetMenu(fileName = "WeaponScriptableObjects", menuName = "ScriptableObjects/Weapon")]
public class WeaponScriptableObject : ScriptableObject
{
    [SerializeField]
    GameObject weaponControllerPrefab;
    public GameObject WeaponControllerPrefab => weaponControllerPrefab;

    [SerializeField]
    GameObject prefab; //just a single icon prefab
    public GameObject Prefab { get => prefab; private set => prefab = value; }

    /* Base Weapon Stats */
    [SerializeField]
    float weaponDamage;
    public float WeaponDamage { get => weaponDamage; private set => weaponDamage = value; }
    [SerializeField]
    float weaponSpeed;
    public float WeaponSpeed { get => weaponSpeed; private set => weaponSpeed = value; }
    [SerializeField]
    float weaponCooldown;
    public float WeaponCooldown { get => weaponCooldown; private set => weaponCooldown = value; }
    [SerializeField]
    int weaponPierce;
    public int WeaponPierce { get => weaponPierce; private set => weaponPierce = value; }
}
