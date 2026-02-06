using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationMatrixBehavior : ProjectileBehavior
{
    protected override void Start()
    {
        base.Start();
    }

    private void Update()
    {
        //飛行
        transform.position += shootingDir * weaponData.WeaponSpeed * Time.deltaTime;
    }

    protected override void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Enemy")) return;

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
}
