using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterController : MonoBehaviour
{
    [Header("Shooting Settings")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float shootInterval = 2f;
    [SerializeField] private float bulletSpeed = 6f;
    [SerializeField] private float bulletDamage = 5f;

    private float shootTimer;
    private Transform player;

    private void Start()
    {
        shootTimer = shootInterval;
    }

    private void Update()
    {
        if (PlayerManager.activePlayer == null) return;

        player = PlayerManager.activePlayer.transform;

        shootTimer -= Time.deltaTime;
        if (shootTimer <= 0f)
        {
            Shoot();
            shootTimer = shootInterval;
        }
    }

    private void Shoot()
    {
        if (bulletPrefab == null || player == null) return;

        Vector2 dir =
            (player.position - transform.position).normalized;

        GameObject bullet =
            Instantiate(bulletPrefab, transform.position, Quaternion.identity);

        EnemyBullet bulletScript = bullet.GetComponent<EnemyBullet>();
        if (bulletScript != null)
        {
            bulletScript.Init(dir, bulletSpeed, bulletDamage);
        }
    }
}
