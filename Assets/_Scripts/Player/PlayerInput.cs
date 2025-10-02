using UnityEngine;
using UnityEngine.InputSystem;

[DisallowMultipleComponent]
public class PlayerInput : SingletonMonobehaviour<PlayerInput>
{

    [Header("Input Actions Asset")]
    [SerializeField] private InputActionAsset inputActions;

    // Input Action
    public InputAction moveAction;

    public InputAction dashAction;

    public InputAction attackAction;

    // Input Value
    public Vector2 moveInput { get; private set; }
    public bool dashWasPressed { get; private set; }
    public bool attackWasPressed { get; private set; }


    protected override void Awake()
    {
        base.Awake();

        var actionMap = inputActions.FindActionMap("Player");

        moveAction = actionMap.FindAction("Movement");
        dashAction = actionMap.FindAction("Dash");
        attackAction = actionMap.FindAction("Attack");
    }

    private void OnEnable()
    {
        moveAction.Enable();
        dashAction.Enable();
        attackAction.Enable();

        dashAction.performed += OnDashPerformed;
        attackAction.performed += OnAttackPerformed;
    }

    private void OnDisable()
    {
        moveAction.Disable();
        dashAction.Disable();
        attackAction.Disable();

        dashAction.performed -= OnDashPerformed;
        attackAction.performed -= OnAttackPerformed;
    }

    private void Update()
    {
        moveInput = moveAction.ReadValue<Vector2>();
    }

    void LateUpdate()
    {
        dashWasPressed = false;
        attackWasPressed = false;
    }

    #region Input events handler

    private void OnDashPerformed(InputAction.CallbackContext context)
    {
        dashWasPressed = true;
    }

    private void OnAttackPerformed(InputAction.CallbackContext context)
    {
        attackWasPressed = true;
    }

    #endregion


    #region Public Helper Methods

    public bool IsMovingHorizontally()
    {
        return Mathf.Abs(moveInput.x) > 0.1f;
    }

    public bool IsMovingVertically()
    {
        return Mathf.Abs(moveInput.y) > 0.1f;
    }

    public float GetHorizontalInput()
    {
        float horizontal = moveInput.x;
        if (horizontal > 0.5f) return 1f;
        if (horizontal < -0.5f) return -1f;
        return 0f;
    }

    public float GetVerticalInput()
    {
        float vertical = moveInput.y;
        if (vertical > 0.5f) return 1f;
        if (vertical < -0.5f) return -1f;
        return 0f;
    }
    #endregion

}