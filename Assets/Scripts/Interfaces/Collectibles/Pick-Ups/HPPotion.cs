using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPPotion : MonoBehaviour, iCollectibles
{
    [SerializeField, Header("Settings")]
    private float restoreHP = 50f;
    private float attractionRange = 1.5f;
    private float attractionSpeed = 5f;

    private Transform targetPlayer;

    private void Update()
    {
        FindNearestPlayer();
        AttractToPlayer();
    }

    private void FindNearestPlayer()
    {
        float minDist = Mathf.Infinity;
        targetPlayer = null;

        foreach (var player in PlayerManager.Instance.players)
        {
            float d = Vector2.Distance(transform.position, player.transform.position);
            if (d < minDist && d <= attractionRange)
            {
                minDist = d;
                targetPlayer = player.transform;
            }
        }
    }

    private void AttractToPlayer()
    {
        if (targetPlayer == null) return;

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPlayer.position,
            attractionSpeed * Time.deltaTime
        );

        float dist = Vector2.Distance(transform.position, targetPlayer.position);
        if (dist <= 0.3f)
        {
            Collect();
        }
    }

    public void Collect()
    {
        if (targetPlayer == null) return;

        //Send to PlayerManager
        PlayerManager.Instance.RestoreHP(restoreHP);

        Destroy(gameObject);
    }
}
