using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputSubscription : MonoBehaviour
{
    //Reference to the auto-generated input actions class
    private PlayerInputActions inputActions;

    //Movement (public so PlayerMovementScript can read it)
    [HideInInspector] public Vector2 moveInput = Vector2.zero;

    //Button actions
    [HideInInspector] public bool skillPressed = false;
    [HideInInspector] public bool attackPressed = false;
    [HideInInspector] public bool interactPressed = false;

    private void Awake()
    {
        inputActions = new PlayerInputActions();

        //Movement
        inputActions.Player.Movement.performed += ctx =>
        {
            moveInput = ctx.ReadValue<Vector2>();
        };
        inputActions.Player.Movement.canceled += ctx =>
        {
            moveInput = Vector2.zero;
        };

        //Skill 
        inputActions.Player.Skill.started += ctx => skillPressed = true;
        inputActions.Player.Skill.canceled += ctx => skillPressed = false;
    }

    private void OnEnable()
    {
        if (inputActions != null)
            inputActions.Enable();
    }

    private void OnDisable()
    {
        if (inputActions != null)
            inputActions.Disable();
    }

    /// <summary>
    /// Helpers for scripts that only want to know if an action was pressed *this frame*.
    /// </summary>
    public bool ConsumeAttackPress()
    {
        if (attackPressed)
        {
            attackPressed = false;
            return true;
        }
        return false;
    }

    public bool ConsumeSkillPress()
    {
        if (skillPressed)
        {
            skillPressed = false;

            SkillBehavior skill = PlayerManager.activePlayer.GetComponent<SkillBehavior>();
            if (skill != null)
                skill.Activate(PlayerManager.activePlayer);

            return true;
        }
        return false;
    }


    public bool ConsumeInteractPress()
    {
        if (interactPressed)
        {
            interactPressed = false;
            return true;
        }
        return false;
    }
}
