using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class EnemyBullet : MonoBehaviour
{
    private Vector2 moveDir;
    private float speed;
    private float damage;

    public float destroyAfterSeconds = 5f;

    public void Init(Vector2 dir, float bulletSpeed, float bulletDamage)
    {
        moveDir = dir.normalized;
        speed = bulletSpeed;
        damage = bulletDamage;

        Destroy(gameObject, destroyAfterSeconds);
    }

    private void Update()
    {
        transform.position += (Vector3)(moveDir * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (PlayerManager.Instance != null)
            {
                Debug.Log($"Bullet hit: {other.name}");

                PlayerManager.Instance.TryApplyDamage
                (
                    other.gameObject,
                    damage,
                    out _
                );
                //撞到玩家就消失
                Destroy(gameObject);
            }
        }
    }
}
