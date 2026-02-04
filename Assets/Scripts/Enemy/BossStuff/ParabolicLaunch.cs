using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParabolicLaunch : MonoBehaviour
{
    private Transform target;
    private Vector2 startPos;
    private Vector2 targetPos;
    private float force;
    private float timer = 0f;
    private float duration = 1.2f; //飛行時間，可依需求調整

    private EnemyController enemyCtrl;
    private Rigidbody2D rb;

    public void Setup(Transform playerTransform, float launchForce)
    {
        target = playerTransform;
        startPos = transform.position;
        targetPos = playerTransform.position; //記錄發射那一刻玩家的位置
        force = launchForce;

        //暫時關閉 AI，等落地再開啟
        enemyCtrl = GetComponent<EnemyController>();
        if (enemyCtrl != null) enemyCtrl.enabled = false;

        rb = GetComponent<Rigidbody2D>();
        if (rb != null) rb.isKinematic = true; //飛行時不參與物理碰撞
    }

    void Update()
    {
        timer += Time.deltaTime;
        float progress = timer / duration;

        if (progress <= 1.0f)
        {
            //線性移動 (X, Y)
            Vector2 currentPos = Vector2.Lerp(startPos, targetPos, progress);

            //加入拋物線高度 (正弦曲線模擬跳躍)
            float height = Mathf.Sin(progress * Mathf.PI) * force;
            transform.position = new Vector3(currentPos.x, currentPos.y + height, 0f);
        }
        else
        {
            Landing();
        }
    }

    void Landing()
    {
        if (enemyCtrl != null) enemyCtrl.enabled = true;
        if (rb != null) rb.isKinematic = true;

        Destroy(this); //移除拋物線腳本，讓小怪恢復正常運作
    }
}