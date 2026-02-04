using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Bloodthirst : SkillBehavior
{
    [Header("Dash Settings")]
    public float dashDistance = 2.5f;
    public float dashSpeed = 20f;

    [Header("Chase Settings")]
    public float chaseDuration = 2f;
    public float chaseAcceleration = 25f;
    public float maxChaseSpeed = 12f;

    [Header("Damage Settings")]
    public float bloodthirstDamage = 20f;

    [Header("Invincibility Settings")]
    private float invincibleDelay = 1f;

    public bool isBloodthirstAttack = true;
    private bool interrupted = false;
    private GameObject owner;
    private TrailRenderer trail;

    public void AttachToPlayer(GameObject player)
    {
        owner = player;
        transform.SetParent(player.transform); //掛上玩家
        transform.localPosition = Vector3.zero; //位置對齊玩家
        transform.localRotation = Quaternion.identity;

        //確保技能有Collider2D並設為trigger
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.isTrigger = true;

        trail = GetComponent<TrailRenderer>();
        if (trail != null)
        {
            trail.startWidth = 0.4f;
            trail.endWidth = 0f;
            trail.material = new Material(Shader.Find("Sprites/Default"));

            //設定半透明漸層
            Gradient g = new Gradient();
            g.SetKeys(
                new GradientColorKey[]
                {
                    new GradientColorKey(new Color(1.0f, 0f, 0f), 0f),
                    new GradientColorKey(new Color(1f, 1f, 1f), 1f)
                },
                new GradientAlphaKey[]
                {
                    new GradientAlphaKey(0.6f, 0f), //起點半透明
                    new GradientAlphaKey(0f, 1f)    //尾端消失
                }
            );

            trail.colorGradient = g;

            trail.emitting = false;   //一開始不要畫
            trail.Clear();
        }
    }

    public override void Activate(GameObject user)
    {
        if (owner == null)
            owner = user;

        owner.GetComponent<MonoBehaviour>()
            .StartCoroutine(BloodthirstRoutine(owner));
    }

    private IEnumerator BloodthirstRoutine(GameObject user)
    {
        Rigidbody2D rb = user.GetComponent<Rigidbody2D>();
        PlayerMovementScript move = user.GetComponent<PlayerMovementScript>();

        if (rb == null || move == null) yield break;
        if (!PlayerManager.Instance.TryGetRuntimeData(user, out PlayerRunTimeData data))
            yield break;

        /* ===== Phase 0：鎖定控制 + 免傷 ===== */
        interrupted = false;
        data.isInvincible = true;
        move.enabled = false;
        rb.velocity = Vector2.zero;
        if (trail != null)
        {
            trail.Clear();
            trail.emitting = true;
        }

        /* ===== Phase 1：直線衝刺 ===== */
        Vector2 mouseDir = GetMouseDir(user.transform.position);
        float traveled = 0f;

        while (traveled < dashDistance)
        {
            float step = dashSpeed * Time.fixedDeltaTime;
            rb.velocity = mouseDir * dashSpeed;
            traveled += step;
            yield return new WaitForFixedUpdate();
        }

        if (interrupted)
        {
            Cleanup(user, rb, move);
            StartCoroutine(EndInvincibleAfterDelay(invincibleDelay));
            yield break;
        }

        /* ===== Phase 2：自動追游標加速滑行 ===== */
        float timer = 0f;
        float currentSpeed = rb.velocity.magnitude;

        while (timer < chaseDuration && !interrupted)
        {
            Vector2 dir = GetMouseDir(user.transform.position);

            currentSpeed += chaseAcceleration * Time.fixedDeltaTime;
            currentSpeed = Mathf.Min(currentSpeed, maxChaseSpeed);

            rb.velocity = dir * currentSpeed;

            timer += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        /* ===== 結束：歸還控制 ===== */
        Cleanup(user, rb, move);
        StartCoroutine(EndInvincibleAfterDelay(invincibleDelay));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (interrupted) return;
        if (owner == null) return;

        EnemyStat enemy = other.GetComponent<EnemyStat>();
        if (enemy == null) return;

        enemy.TakeDamage(bloodthirstDamage, "Bloodthirst");

        interrupted = true;

        //擊中效果跟音效
        if (trail != null)
        {
            trail.time = 0.05f; //瞬間收短
        }

    }

    private void Cleanup(GameObject user, Rigidbody2D rb, PlayerMovementScript move)
    {
        rb.velocity = Vector2.zero;
        move.enabled = true;
        if (trail != null)
            trail.emitting = false;
    }

    private IEnumerator EndInvincibleAfterDelay(float delay)
    {
        if (owner != null && PlayerManager.Instance.TryGetRuntimeData(owner, out var data))
        {
            yield return new WaitForSeconds(delay);
            data.isInvincible = false;
        }
        if (trail != null)
        {
            trail.emitting = false;
            trail.Clear();
        }
        Destroy(this.gameObject);
    }

    private Vector2 GetMouseDir(Vector3 from)
    {
        Vector3 mouseWorld =
            Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0f;
        return (mouseWorld - from).normalized;
    }

    public void ResetCooldown()
    {
        if (owner != null && PlayerManager.Instance.TryGetRuntimeData(owner, out var data))
        {
            data.skillCooldownTimer = 0f; 
            Debug.Log($"{data.baseData.playerName} 的 Bloodthirst 已重置");
        }
    }
}
