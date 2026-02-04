using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableProps : MonoBehaviour
{
    public static event System.Action<BreakableProps> OnPropDestroyed;

    public float propHealth = 5;

    public void PropTakeDamage(float dmg)
    {
        propHealth -= dmg;
        if(propHealth <= 0 )
        {
            KillProp();
        }
    }

    public void KillProp()
    {
        OnPropDestroyed?.Invoke(this);
        Destroy(gameObject);
    }
}
