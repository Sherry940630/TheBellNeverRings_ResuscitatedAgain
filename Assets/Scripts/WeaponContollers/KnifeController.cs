using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class KnifeController : WeaponController 
{  
    protected override void Start() 
    { 
        base.Start(); 
    } 
    protected override void Attack() 
    { 
        base.Attack(); 
        GameObject spawnKnife = Instantiate(prefab, transform.position, Quaternion.identity); 
        Vector2 dir = playerMovement.lookDir; 
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg; 
        spawnKnife.transform.rotation = Quaternion.Euler(0, 0, angle); //spawnKnife
        //spawnKnife.GetComponent<KnifeBehavioir>().Initialize(this);
        spawnKnife.GetComponent<KnifeBehavioir>().DirectionCheck(dir); 
    } 
}