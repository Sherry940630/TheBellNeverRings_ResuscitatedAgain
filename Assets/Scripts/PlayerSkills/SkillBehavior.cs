using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
   WHAT IS ABSTRACT CLASS:
       1. It's a template for other classes to inherit.
       2. It can't be instantiated directly (you can¡¦t create an object from it).
   REASONS TO USE ABSTRACT CLASS:
       1. To enforce structure ¡X any skill must have an Activate() method.
       2. To share code ¡X for example, all skills can use PrintSkillInfo().
 */

public abstract class SkillBehavior : MonoBehaviour
{
    public abstract void Activate(GameObject user);
}
