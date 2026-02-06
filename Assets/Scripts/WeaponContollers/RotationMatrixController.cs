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
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        matrix.transform.rotation = Quaternion.Euler(0, 0, angle);

        RotationMatrixBehavior behavior = matrix.GetComponent<RotationMatrixBehavior>();
        behavior.DirectionCheck(dir);
    }
}
