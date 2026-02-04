using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovementScript : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float movingSpeed = 5f;

    [Header("Collision Settings")]
    [SerializeField] private LayerMask solidObjectsLayer;
    [SerializeField] private LayerMask airWallLayer;
    [SerializeField] private float collisionCheckRadius = 0.2f;

    [Header("Ground Check")]
    [SerializeField] private LayerMask bgLayer;
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private float groundCheckRadius = 0.3f;

    private Rigidbody2D rb;

    [HideInInspector] public Vector2 lookDir = Vector2.right;
    [HideInInspector] public Vector2 moveDir = Vector2.zero;
    [HideInInspector] public Vector2 lastMovedVector = Vector2.right; // default face direction is right
    [HideInInspector] public float lastHorizontalVector = 0f;
    [HideInInspector] public float lastVerticalVector = 0f;

    private PlayerInputSubscription inputSubscription;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        //Try to get the input subscription on this object or a parent (flexible for different setups)
        inputSubscription = GetComponent<PlayerInputSubscription>();
        if (inputSubscription == null)
            inputSubscription = GetComponentInParent<PlayerInputSubscription>();

        //Initialize last move direction to the right (consistent with original)
        lastMovedVector = new Vector2(1f, 0f);
        lastHorizontalVector = lastMovedVector.x;
        lastVerticalVector = lastMovedVector.y;
    }

    private void Update()
    {
        HandleInput();
        HandleSkill();
    }

    private void FixedUpdate()
    {
        HandleMovement();
        UpdateLookDirection();
    }

    /// <summary>
    /// Read input either from new input subscription or fallback to old Input axes.
    /// Also updates lastMovedVector and last horizontal/vertical vectors using the original logic.
    /// </summary>
    private void HandleInput()
    {
        Vector2 rawInput = Vector2.zero;

        if (inputSubscription != null)
        {
            // Use new input system via subscription
            rawInput = inputSubscription.moveInput;
        }
        else
        {
            // Fallback for compatibility: legacy input
            float moveX = UnityEngine.Input.GetAxisRaw("Horizontal");
            float moveY = UnityEngine.Input.GetAxisRaw("Vertical");
            rawInput = new Vector2(moveX, moveY);
        }

        // Normalize to preserve movement speed consistency
        moveDir = rawInput.normalized;

        // Preserve original last-move logic exactly:
        // - If horizontal movement occurs, store lastHorizontalVector and set lastMovedVector to (x,0)
        // - If vertical movement occurs, store lastVerticalVector and set lastMovedVector to (0,y)
        // - If both occur (diagonal), store both and set lastMovedVector to (x,y)
        if (moveDir.x != 0f)
        {
            lastHorizontalVector = moveDir.x;
            // set horizontal-only vector
            lastMovedVector = new Vector2(lastHorizontalVector, 0f);
        }

        if (moveDir.y != 0f)
        {
            lastVerticalVector = moveDir.y;
            // set vertical-only vector
            lastMovedVector = new Vector2(0f, lastVerticalVector);
        }

        // diagonal case override: if both axes nonzero, set full vector
        if (moveDir.x != 0f && moveDir.y != 0f)
        {
            lastHorizontalVector = moveDir.x;
            lastVerticalVector = moveDir.y;
            lastMovedVector = new Vector2(lastHorizontalVector, lastVerticalVector);
        }

        // If not moving, keep previous lastMovedVector (unchanged)
    }

    private void HandleMovement()
    {
        /*
        if (!IsOnGround())
        {
            rb.velocity = Vector2.zero;
            return;
        }
        */

        if (moveDir != Vector2.zero)
        {
            //在移動目標點檢查是否有固體層級的物件
            Vector3 targetPos = transform.position + (Vector3)(moveDir * movingSpeed * Time.fixedDeltaTime);

            if (!IsWalkable(targetPos))
            {
                rb.velocity = Vector2.zero; //撞到了，停止移動
                return;
            }
            if(TouchAirWall(targetPos))
            {
                rb.velocity = Vector2.zero; //撞到了，停止移動
                Debug.Log("Touched Air Wall");
                return;
            }
        }
        rb.velocity = moveDir * movingSpeed;
    }

    private void HandleSkill()
    {
        if (!inputSubscription.ConsumeSkillPress()) return;

        //取得 runtime data
        if (!PlayerManager.Instance.TryGetRuntimeData(PlayerManager.activePlayer, out PlayerRunTimeData runData))
        {
            Debug.LogError("No runtime data for player!");
            return;
        }

        //技能 prefab
        SkillBehavior skillPrefab = runData.baseData.manualSkill;
        if (skillPrefab == null)
        {
            Debug.LogWarning("No manual skill assigned!");
            return;
        }

        //cooldown
        if (runData.skillCooldownTimer > 0)
        {
            Debug.Log("Skill on cooldown!");
            return;
        }

        //產生技能
        SkillBehavior skillInstance = Instantiate(skillPrefab);
        //針對Bloodthirst掛上玩家
        if (skillInstance is Bloodthirst bt)
        {
            bt.AttachToPlayer(PlayerManager.activePlayer);

            Rigidbody2D rbSkill = skillInstance.GetComponent<Rigidbody2D>();
            if (rbSkill == null) rbSkill = skillInstance.gameObject.AddComponent<Rigidbody2D>();
            rbSkill.isKinematic = true; //只移動，不受物理
            rbSkill.gravityScale = 0;

            Collider2D col = skillInstance.GetComponent<Collider2D>();
            if (col == null) col = skillInstance.gameObject.AddComponent<BoxCollider2D>();
            col.isTrigger = true;
        }
        skillInstance.Activate(PlayerManager.activePlayer);

        //重設冷卻
        runData.skillCooldownTimer = 3f;
    }

    private void UpdateLookDirection()
    {
        if (MouseAimController.Instance == null) return;

        lookDir = MouseAimController.Instance.GetAimDirFrom(transform.position);

        if (lookDir != Vector2.zero)
        {
            lastHorizontalVector = lookDir.x;
            lastVerticalVector = lookDir.y;
        }
    }

    private bool IsWalkable(Vector2 targetPos)
    {
        return (Physics2D.OverlapCircle(targetPos, collisionCheckRadius, solidObjectsLayer) == null);
    }

    private bool TouchAirWall(Vector2 targetPos)
    {
        return (Physics2D.OverlapCircle(targetPos, collisionCheckRadius, airWallLayer) == null);
    }

    private void OnTriggerEnter2D(Collider2D enem)
    {
        if (enem == null) return;

        EnemyController enemy = enem.GetComponent<EnemyController>();
        if (enemy != null)
        {
            if (PlayerManager.Instance == null)
            {
                Debug.LogWarning("PlayerManager.Instance is null when applying collision damage.");
                return;
            }

            int playerIndex = PlayerManager.Instance.players.IndexOf(gameObject);
            if (playerIndex < 0 || playerIndex >= PlayerManager.Instance.runtimeDataList.Count)
            {
                Debug.LogWarning("Player index invalid or runtime data missing when applying collision damage.");
                return;
            }

            PlayerRunTimeData runtimeData = PlayerManager.Instance.runtimeDataList[playerIndex];

            if (runtimeData.isInvincible)
            {
                Debug.Log($"{runtimeData.baseData.playerName} is currently INVINCIBLE! Damage nullified.");
                return;
            }

            runtimeData.currentHealth -= enemy.enemyData.Damage;
            Debug.Log($"{runtimeData.baseData.playerName} Health: {runtimeData.currentHealth}");
        }
    }

#if UNITY_EDITOR
    //Draw the lastMovedVector in the editor for debugging
    private void OnDrawGizmosSelected()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = Color.green;
            Vector3 from = transform.position;
            Gizmos.DrawLine(from, from + (Vector3)lastMovedVector);
        }
    }
#endif
}
