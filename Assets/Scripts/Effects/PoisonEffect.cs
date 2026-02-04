using UnityEngine;

public class PoisonEffect : ContinuousDamageEffect
{
    [Header("Poison Visual")]
    public GameObject poisonVfxPrefab;
    private GameObject vfxInstance;

    private float damagePerTick;
    private float remainingDuration;

    private float tickInterval = 1f;
    private float timer;

    private EnemyStat enemy;

    public void Init(float dmgPerTick, float duration, GameObject vfxPrefab)
    {
        damagePerTick = dmgPerTick;
        remainingDuration = duration;
        enemy = GetComponent<EnemyStat>();
        poisonVfxPrefab = vfxPrefab;

        if (poisonVfxPrefab != null)
        {
            vfxInstance = Instantiate(
                poisonVfxPrefab,
                transform.position,
                Quaternion.identity,
                transform 
            );
        }
    }

    private void Update()
    {
        if (enemy == null) return;

        timer += Time.deltaTime;
        if (timer >= tickInterval)
        {
            timer = 0f;

            enemy.TakeDamage(damagePerTick);
            remainingDuration -= tickInterval;

            if (remainingDuration <= 0f)
            {
                Cleanup();
            }
        }
    }

    public override float GetRemainingDamage()  //for Laplace Transform
    {
        int remainingTicks = Mathf.CeilToInt(remainingDuration / tickInterval);
        return remainingTicks * damagePerTick;
    }

    public override void CancelEffect()
    {
        Cleanup();
    }

    void Cleanup()
    {
        if (vfxInstance != null)
            Destroy(vfxInstance);

        Destroy(this);
    }

}
