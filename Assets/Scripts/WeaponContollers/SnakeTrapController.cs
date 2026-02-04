using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeTrapController : WeaponController
{
    protected override void Attack()
    {
        base.Attack();
        GameObject trap = Instantiate(prefab, transform.position, Quaternion.identity);
        trap.GetComponent<SnakeTrapBehavior>().ownerPlayer = playerMovement.gameObject;
    }
}

