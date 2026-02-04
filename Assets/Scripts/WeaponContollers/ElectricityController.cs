using UnityEngine;
using System.Linq;
public class ElectricityController : WeaponController 
{ 
    [Header("Electricity Visuals")] 
    public GameObject lightningPrefab; 
    public GameObject electricSplatterPrefab; 
    [Header("Stats")] private float tickInterval = 0.3f; 
    private float spreadRange = 2f; 
    private float duration = 2f; 
    protected override void Attack() 
    { 
        base.Attack(); 
        EnemyStat nearest = FindNearestEnemy(); 
        if (nearest == null) return; 
        nearest.TakeDamage(weaponData.WeaponDamage); //Lightning Visual
        LightningBoltAnimation bolt = Instantiate(lightningPrefab) .GetComponent<LightningBoltAnimation>(); 
        bolt.Setup(transform.position, nearest.transform.position); //Splatter Effect (spawn once + auto destroy)
        if (electricSplatterPrefab != null)
        { 
            Destroy( Instantiate(electricSplatterPrefab, nearest.transform.position, Quaternion.identity), 0.2f ); 
        } //Spread the electricity
            ApplyElectricCharge(nearest); 
    } 
    EnemyStat FindNearestEnemy() 
    { 
        EnemyStat[] enemies = FindObjectsOfType<EnemyStat>(); 
        if (enemies.Length == 0) 
            return null; 
        return enemies .OrderBy(e => Vector3.Distance(transform.position, e.transform.position)) .FirstOrDefault(); 
    } 
    void ApplyElectricCharge(EnemyStat target) 
    { 
        ElectricChargeEffect eff = target.GetComponent<ElectricChargeEffect>(); 
        if (eff == null) 
        { 
            eff = target.gameObject.AddComponent<ElectricChargeEffect>(); 
            eff.lightningPrefab = lightningPrefab;
            eff.Initialize( 
            weaponData.WeaponDamage * 0.5f, //damage per tick
            tickInterval, 
            spreadRange, 
            duration ); 
        } 
    } 
}