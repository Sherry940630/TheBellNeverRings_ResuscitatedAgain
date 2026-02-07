using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationMatrixController : WeaponController
{
    protected override void Attack()
    {
        base.Attack();

        GameObject matrix = Instantiate(prefab, transform.position, Quaternion.identity);

        Vector2 dir = playerMovement.lookDir;
        RotationMatrixBehavior behavior = matrix.GetComponent<RotationMatrixBehavior>();
        behavior.DirectionCheck(dir);
    }
}
