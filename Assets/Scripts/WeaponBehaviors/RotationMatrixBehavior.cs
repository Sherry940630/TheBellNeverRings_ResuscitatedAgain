using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationMatrixBehavior : ProjectileBehavior
{
    public float flyDuration = 0.3f;
    private bool canMove = true;

    protected override void Start()
    {
        base.Start();
        StartCoroutine(StopAfterTime());
    }

    private void Update()
    {
        if (!canMove) return;
        //飛行
        transform.position += shootingDir * weaponData.WeaponSpeed * Time.deltaTime;
    }

    protected override void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Enemy")) return;

        if (col.GetComponent<BossController>() != null)
        {
            Debug.Log("Rotation Matrix 撞擊 Boss，效果無效。");
            return;
        }

        EnemyStat enemy = col.GetComponent<EnemyStat>();
        if (enemy == null) return;

        ApplyRotation(enemy);

        Destroy(gameObject);
    }

    private void ApplyRotation(EnemyStat enemy)
    {
        EnemyController ec = enemy.GetComponent<EnemyController>();
        if (ec == null) return;

        ec.ApplyReverse(10f); // 10 秒後消失

        // 未來擴充點：
        // - 改成轉任意角度
        // - 範圍搜尋多個 enemy 呼叫這個 function
    }

    private IEnumerator StopAfterTime()
    {
        yield return new WaitForSeconds(flyDuration);
        canMove = false;
    }
}
