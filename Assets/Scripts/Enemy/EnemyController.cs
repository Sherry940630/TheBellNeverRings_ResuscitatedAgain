using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public EnemyScriptableObject enemyData;
    GameObject player;
    private float currentHealth;
    private bool isReversed = false;
    public bool IsReversed => isReversed;

    void Start()
    {
        player = PlayerManager.activePlayer;
        currentHealth = enemyData.MaxHealth;
    }

    private void Update()
    {
        player = PlayerManager.activePlayer;

        Vector2 dir;

        if (!isReversed)
        {
            dir = (player.transform.position - transform.position).normalized;
        }
        else
        {
            dir = (transform.position - player.transform.position).normalized;
        }

        transform.position +=
        (Vector3)(dir * enemyData.MoveSpeed * Time.deltaTime);
    }

    public void ApplyReverse(float duration)
    {
        if (gameObject.CompareTag("Boss")) return;

        StopAllCoroutines();
        StartCoroutine(ReverseRoutine(duration));
    }

    private IEnumerator ReverseRoutine(float duration)
    {
        isReversed = true;
        yield return new WaitForSeconds(duration);
        Destroy(gameObject); //10秒後敵人消失
    }
}
