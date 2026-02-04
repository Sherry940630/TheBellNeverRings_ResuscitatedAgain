using UnityEngine;

public abstract class ContinuousDamageEffect : MonoBehaviour
{
    public float damagePerTick;
    public float tickInterval;
    public float duration;

    //How much total damage is remaining if all ticks run normally
    public virtual float GetRemainingDamage()
    {
        return damagePerTick * (duration / tickInterval);
    }

    public abstract void CancelEffect();
}
