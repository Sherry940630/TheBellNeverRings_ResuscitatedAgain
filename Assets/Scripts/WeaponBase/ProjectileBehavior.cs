using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ProjectileBehavior : MonoBehaviour 
{ 
    public WeaponScriptableObject weaponData; 
    protected Vector3 shootingDir; 
    public float destroyAfterSeconds; 
    protected float currentDamage; 
    protected float currentSpeed; 
    protected float currentCooldown; 
    public int pierce;
    private HashSet<EnemyStat> hitEnemies = new HashSet<EnemyStat>();

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
    public void DirectionCheck(Vector3 dir) 
    { 
        shootingDir = dir; 
        float dirX = shootingDir.x; 
        float dirY = shootingDir.y; 
        Vector3 scale = transform.localScale; //transform.localScale會回傳附加此腳本的遊戲物件的尺寸 
        Vector3 rotation = transform.rotation.eulerAngles; 
    } 
    protected virtual void OnTriggerEnter2D(Collider2D coll)
    {   //this function triggers automatically by Unity, not manually
        if (this == null) return;

        if (coll.CompareTag("Enemy")) 
        {
            EnemyStat enemy = coll.GetComponentInParent<EnemyStat>();
            if (enemy != null && !hitEnemies.Contains(enemy))
            {
                hitEnemies.Add(enemy);
                enemy.TakeDamage(currentDamage);
                //ReducePierce();
            }
        }
        else if (coll.CompareTag("Prop"))
        {
            if(coll.TryGetComponent(out BreakableProps breakableProp))
            {
                breakableProp.PropTakeDamage(currentDamage);
                //ReducePierce();
            }
        }
    } 
    /*
    void ReducePierce()
    {
        pierce--;

        if (pierce <= 0)
        {
            Collider2D myCol = GetComponent<Collider2D>();
            if (myCol != null) myCol.enabled = false;
            Destroy(gameObject);
        }
    }
    */
}