using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovementScript))]
public class InactiveFollowController : MonoBehaviour
{
    public float followDistance = 5f;
    public float stopDistance = 2f;

    private PlayerMovementScript movement;
    private Transform activeTarget;

    void Awake()
    {
        movement = GetComponent<PlayerMovementScript>();
    }

    void Update()
    {
        if (PlayerManager.activePlayer == null) return;

        if (gameObject == PlayerManager.activePlayer)
            return;

        activeTarget = PlayerManager.activePlayer.transform;

        float dist = Vector2.Distance(transform.position, activeTarget.position);

        if (dist > followDistance)
        {
            //往active player方向移動
            float t = Mathf.InverseLerp(stopDistance, followDistance, dist);
            Vector2 dir = (activeTarget.position - transform.position).normalized;
            movement.moveDir = dir * t;
        }
        else if (dist < stopDistance)
        {
            movement.moveDir = Vector2.zero;
        }
    }
}
