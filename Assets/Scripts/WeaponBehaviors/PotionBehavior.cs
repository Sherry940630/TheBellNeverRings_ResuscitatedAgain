using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PotionBehavior : ProjectileBehavior 
{ 
    PotionController potionController; 
    private Vector3 velocity; private float gravity = -9.8f;
    private float verticalSpeed; 
    [Header("Throw Settings")] 
    public float throwForce = 8f; 
    public float throwUpwardForce = 6f; 
    protected override void Start() 
    { 
        base.Start(); 
        potionController = GetComponentInParent<PotionController>(); 
        if (potionController == null) 
            potionController = FindObjectOfType<PotionController>(); 
    } 
    private void Update() 
    { 
        verticalSpeed += gravity * Time.deltaTime; 
        Vector3 move = (shootingDir * throwForce + Vector3.up * verticalSpeed) * Time.deltaTime; 
        transform.position += move; 
    } 
    public void DirectionCheckAndThrow(Vector2 dir) 
    { 
        DirectionCheck(dir); 
        shootingDir = dir.normalized; 
        verticalSpeed = throwUpwardForce; 
    } 
}