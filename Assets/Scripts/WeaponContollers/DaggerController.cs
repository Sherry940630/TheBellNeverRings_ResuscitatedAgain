using UnityEngine;

public class FlyingDaggerController : WeaponController
{
    protected override void Attack()
    {
        base.Attack();

        GameObject dagger = Instantiate(prefab, transform.position, Quaternion.identity);

        Vector2 randomDir = Random.insideUnitCircle.normalized;
        dagger.GetComponent<DaggerBehavior>().Init(randomDir);
    }
}