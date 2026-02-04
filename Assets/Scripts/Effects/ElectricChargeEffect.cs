using UnityEngine;
using System.Collections;

public class ElectricChargeEffect : ContinuousDamageEffect
{
    float linkRange;
    EnemyStat ownerStat;
    bool isRunning = false;

    public GameObject lightningPrefab;

    public void Initialize(float dmg, float interval, float link, float dur)
    {
        damagePerTick = dmg;
        tickInterval = interval;
        linkRange = link;
        duration = dur;

        ownerStat = GetComponent<EnemyStat>();

        if (!isRunning)
        {
            isRunning = true;
            StartCoroutine(EffectRoutine());
        }
    }

    IEnumerator EffectRoutine()
    {
        float t = 0f;

        while (t < duration)
        {
            ZapAndSpread();
            yield return new WaitForSeconds(tickInterval);
            t += tickInterval;
        }

        Destroy(this);
    }

    public override void CancelEffect()
    {
        StopAllCoroutines();
        Destroy(this);
    }

    void ZapAndSpread()
    {
        ownerStat.TakeDamage(damagePerTick);

        EnemyStat[] enemies = FindObjectsOfType<EnemyStat>();

        foreach (var e in enemies)
        {
            if (e == ownerStat) continue;

            float dist = Vector3.Distance(ownerStat.transform.position, e.transform.position);

            if (dist <= linkRange)
            {
                //Lightning visual
                if (lightningPrefab != null)
                {
                    LightningBoltAnimation bolt =
                        Instantiate(lightningPrefab)
                        .GetComponent<LightningBoltAnimation>();
                    bolt.Setup(ownerStat.transform.position, e.transform.position);
                }

                //Chain effect
                ElectricChargeEffect eff = e.GetComponent<ElectricChargeEffect>();
                if (eff == null)
                {
                    eff = e.gameObject.AddComponent<ElectricChargeEffect>();
                    eff.lightningPrefab = lightningPrefab;
                    eff.Initialize(damagePerTick, tickInterval, linkRange, duration);
                }
            }
        }
    }
}
