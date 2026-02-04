using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossCountdown : MonoBehaviour
{
    [Header("Boss Setup")]
    public GameObject bossPrefab; 
    public Transform bossSpawnPoint; 

    public Text countdownText;
    public float countdownTime = 180f;
    public SpawnEnemy enemySpawner;

    private float timer;
    private bool finished = false;

    private void Start()
    {
        timer = countdownTime;
    }

    private void Update()
    {
        if (finished) return;

        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            EndCountdown();
            return;
        }

        int minutes = Mathf.FloorToInt(timer / 60f);
        int seconds = Mathf.FloorToInt(timer % 60f);

        countdownText.text = $"BOSS FIGHT IN:\n{minutes:00}:{seconds:00}";
    }

    private void EndCountdown()
    {
        finished = true;
        countdownText.text = "";

        CamMovement.Instance.SmoothZoom
        (
            targetValue: 6f,   //OrthographicSize預設是3f
            duration: 1.2f      //拉遠時間
        );

        //停止刷怪
        enemySpawner.CancelInvoke();
        enemySpawner.gameObject.SetActive(false);

        //清空敵人
        EnemyStat[] enemies = FindObjectsOfType<EnemyStat>();
        foreach (EnemyStat e in enemies)
        {
            e.Kill();
        }

        if (bossPrefab != null)
        {
            Vector3 spawnPos = bossSpawnPoint != null ? bossSpawnPoint.position : Vector3.zero;
            Instantiate(bossPrefab, spawnPos, Quaternion.identity);
        }
    }

    public void ForceBossSpawn()
    {
        timer = 0f; 
        Debug.Log("Debug Menu: Boss timer forced to zero.");
    }
}
