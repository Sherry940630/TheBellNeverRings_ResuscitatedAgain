using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeaponBehavior : MonoBehaviour
{
    public WeaponScriptableObject weaponData;
    protected Vector3 shootingDir;
    public float destroyAfterSeconds; 
    protected float currentDamage;
    protected float currentSpeed;
    protected float currentCooldown;
    public int pierce;

    protected void Awake()
    {
        currentDamage = weaponData.WeaponDamage;
        currentSpeed = weaponData.WeaponSpeed;
        currentCooldown = weaponData.WeaponCooldown;
        pierce = weaponData.WeaponPierce;
    }
    protected virtual void Start()
    {
        Destroy(gameObject, destroyAfterSeconds);
    }

    protected virtual void OnTriggerEnter2D(Collider2D coll)
    { //this function triggers automatically by Unity, not manually
        if (coll.CompareTag("Enemy"))
        {
            EnemyStat enemy = coll.GetComponent<EnemyStat>();
            enemy.TakeDamage(currentDamage);
            Destroy(gameObject);
        }
        else if (coll.CompareTag("Prop"))
        {
            if (coll.TryGetComponent(out BreakableProps breakableProp))
            {
                breakableProp.PropTakeDamage(currentDamage);
            }
        }
    }
}
