using UnityEngine;

public class DaggerBehavior : ProjectileBehavior
{
    [SerializeField]
    public int maxBounce = 10;       
    private int currentBounce;

    private Vector2 moveDir;

    public void Init(Vector2 dir) //初始方向
    {
        moveDir = dir.normalized;
        pierce = weaponData.WeaponPierce;
    }

    private void Update()
    {
        transform.position += (Vector3)moveDir * weaponData.WeaponSpeed * Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision) //牆壁反彈
    {
        if (collision.collider.CompareTag("Wall"))
        {
            Vector2 normal = collision.contacts[0].normal;
            moveDir = Vector2.Reflect(moveDir, normal);

            currentBounce++;
            if (currentBounce >= maxBounce)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col) //敵人穿透
    {
        if (col.CompareTag("Enemy"))
        {
            EnemyStat enemy = col.GetComponent<EnemyStat>();
            enemy.TakeDamage(weaponData.WeaponDamage);

            pierce--;
            if (pierce <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}