using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelController : WeaponController
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void Attack()
    {
        base.Attack();
        GameObject spawnWheel = Instantiate(prefab);
        spawnWheel.transform.position = transform.position; //follow player
        spawnWheel.transform.parent = transform;
    }
}
