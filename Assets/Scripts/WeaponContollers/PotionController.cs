using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PotionController : WeaponController 
{ 
    protected override void Start() 
    { base.Start(); } 
    protected override void Attack() 
    { 
        base.Attack(); 
        GameObject spawnedPotion = Instantiate(prefab, transform.position, Quaternion.identity);
        Vector2 dir = playerMovement.lookDir;
        if (playerMovement == null) Debug.LogError("playerMovement is NULL!"); 
        spawnedPotion.GetComponent<PotionBehavior>().DirectionCheckAndThrow(dir); 
    } 
}