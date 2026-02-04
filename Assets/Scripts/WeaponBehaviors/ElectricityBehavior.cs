using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricityBehavior : ProjectileBehavior
{
    protected override void Start()
    {
        base.Start();
    }

    private void Update()
    {
        transform.position += shootingDir * weaponData.WeaponSpeed * Time.deltaTime;
    }

    protected override void OnTriggerEnter2D(Collider2D col)
    {
        EnemyStat enemy = col.GetComponent<EnemyStat>();
        if (enemy != null)
        {
            enemy.TakeDamage(weaponData.WeaponDamage);
            Destroy(gameObject);
        }
    }
}
