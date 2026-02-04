using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeTrapBehavior : MonoBehaviour
{
    public float lifeTime = 10f;
    public float poisonDamage = 2f;
    public float poisonDuration = 5f;
    public GameObject poisonVfxPrefab;
    public GameObject ownerPlayer;

    private bool triggered = false;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D col) //¼Ä¤H½ò¨ì¤¤¬r
    {
        if (triggered) return;

        if (col.CompareTag("Enemy"))
        {
            triggered = true;

            EnemyStat enemy = col.GetComponent<EnemyStat>();
            enemy.gameObject.AddComponent<PoisonEffect>()
                .Init(poisonDamage, poisonDuration, poisonVfxPrefab);

            Destroy(gameObject);
        }
    }

}
