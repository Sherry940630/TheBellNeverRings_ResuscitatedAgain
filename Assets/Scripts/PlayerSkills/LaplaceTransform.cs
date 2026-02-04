using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class LaplaceTransform : SkillBehavior
{
    [Header("Laplace Settings")]
    public float burstMultiplier = 3f;

    public override void Activate(GameObject user)
    {
        Debug.Log("Laplace Transform activated!");

        //Find all enemies
        ContinuousDamageEffect[] effects =
            FindObjectsOfType<ContinuousDamageEffect>();

        foreach (var effect in effects)
        {
            EnemyStat target = effect.GetComponent<EnemyStat>();
            if (target == null) continue;

            float remainingDamage = effect.GetRemainingDamage();
            float burst = remainingDamage * burstMultiplier;

            target.TakeDamage(burst);

            effect.CancelEffect();

            Debug.Log($"Laplace Burst: {target.name} took {burst}!");
        }

        //Trigger screen animation
        var anim = FindObjectOfType<LaplaceTransformAnimation_Cinemachine>();
        if (anim != null)
            anim.PlayEffect();
    }
}
