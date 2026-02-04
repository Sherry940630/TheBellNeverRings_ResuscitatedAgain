using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeBehavioir : ProjectileBehavior
{
    public float spinSpeed = 720f;

    protected override void Start()
    {
        base.Start();
    }

    private void Update()
    {
        //Move the shuriken
        transform.position += shootingDir * weaponData.WeaponSpeed * Time.deltaTime;

        //Spin continuously
        transform.Rotate(0f, 0f, spinSpeed * Time.deltaTime);
    }
}
