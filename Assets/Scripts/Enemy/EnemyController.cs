using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public EnemyScriptableObject enemyData;
    GameObject player;
    private float currentHealth;

    void Start()
    {
        player = PlayerManager.activePlayer;
        currentHealth = enemyData.MaxHealth;
    }

    private void Update()
    {
        player = PlayerManager.activePlayer;
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, enemyData.MoveSpeed*Time.deltaTime);
    }
}
