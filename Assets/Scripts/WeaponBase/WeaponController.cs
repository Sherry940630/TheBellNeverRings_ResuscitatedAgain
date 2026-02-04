using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class WeaponController : MonoBehaviour 
{ 
    [Header("Weapon Stats")] public GameObject prefab; 
    public WeaponScriptableObject weaponData; 
    private float currentCooldown; 
    protected PlayerMovementScript playerMovement; 
    /* 
       virtual allows child classes to override these functions, 
       protected makes it only accessible to child classes. 
    */ 
    protected virtual void Start() 
    { 
        playerMovement = GetComponentInParent<PlayerMovementScript>(); 
        if (playerMovement == null) 
            playerMovement = FindObjectOfType<PlayerMovementScript>(); 
        currentCooldown = weaponData.WeaponCooldown; 
    } 
    protected virtual void Update() 
    { 
        currentCooldown -= Time.deltaTime; 
        if(currentCooldown <= 0f) 
        { 
            Attack(); 
        } 
    } 
    protected virtual void Attack() 
    { 
        currentCooldown = weaponData.WeaponCooldown; 
    } 
}