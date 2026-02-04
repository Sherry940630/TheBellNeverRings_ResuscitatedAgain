using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimator : MonoBehaviour
{
    private Animator anim;
    private PlayerMovementScript movement;

    private readonly int moveXHash = Animator.StringToHash("MoveX");
    private readonly int moveYHash = Animator.StringToHash("MoveY");
    private readonly int speedHash = Animator.StringToHash("Speed");
    private readonly int lastMoveXHash = Animator.StringToHash("LastMoveX");
    private readonly int lastMoveYHash = Animator.StringToHash("LastMoveY");

    void Awake()
    {
        anim = GetComponent<Animator>();

        // Find movement script (could be on same object or parent)
        movement = GetComponent<PlayerMovementScript>();
        if (movement == null)
            movement = GetComponentInParent<PlayerMovementScript>();
    }

    void Update()
    {
        if (movement == null)
            return;

        Vector2 moveDir = movement.moveDir;

        
        if (movement.moveDir.x != 0 || movement.moveDir.y != 0) 
        { 
            anim.SetBool("IsRunning", true); 
            SpriteDirectionChecker(); 
        } 
        else 
        { 
            anim.SetBool("IsRunning", false); 
        }
    }
    private void SpriteDirectionChecker()
    {
        if (movement.lookDir.x > 0)
            transform.localScale = new Vector3(-1, 1, 1);
        else if (movement.lookDir.x < 0)
            transform.localScale = new Vector3(1, 1, 1);
    }
}
