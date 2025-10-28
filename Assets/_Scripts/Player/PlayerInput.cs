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

    public InputAction reloadAction;

    public InputAction scrollWeaponAction;

    public InputAction selectWeapon1Action;

    public InputAction selectWeapon2Action;

    // Input Value
    public Vector2 moveInput { get; private set; }
    public bool dashWasPressed { get; private set; }
    public bool attackWasPressed { get; private set; }
    public bool attackIsHeld { get; private set; }
    public bool attackWasReleased { get; private set; }
    public bool reloadWasPressed { get; private set; }

    public float mouseScrollInput { get; private set; }
    public bool selectWeapon1WasPressed { get; private set; }
    public bool selectWeapon2WasPressed { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        var actionMap = inputActions.FindActionMap("Player");

        moveAction = actionMap.FindAction("Movement");
        dashAction = actionMap.FindAction("Dash");
        attackAction = actionMap.FindAction("Attack");
        reloadAction = actionMap.FindAction("Reload");

        scrollWeaponAction = actionMap.FindAction("ScrollWeapon");
        selectWeapon1Action = actionMap.FindAction("SelectWeapon1");
        selectWeapon2Action = actionMap.FindAction("SelectWeapon2");
    }

    private void OnEnable()
    {
        moveAction.Enable();
        dashAction.Enable();
        attackAction.Enable();
        reloadAction.Enable();

        scrollWeaponAction.Enable();
        selectWeapon1Action.Enable();
        selectWeapon2Action.Enable();

        dashAction.performed += OnDashPerformed;
        attackAction.performed += OnAttackPerformed;
        attackAction.canceled += OnAttackCanceled;
        reloadAction.performed += OnReloadPerformed;

        selectWeapon1Action.performed += OnSelectWeapon1Performed;
        selectWeapon2Action.performed += OnSelectWeapon2Performed;

    }

    private void OnDisable()
    {
        moveAction.Disable();
        dashAction.Disable();
        attackAction.Disable();
        reloadAction.Disable();

        scrollWeaponAction.Disable();
        selectWeapon1Action.Disable();
        selectWeapon2Action.Disable();

        dashAction.performed -= OnDashPerformed;
        attackAction.performed -= OnAttackPerformed;
        attackAction.canceled -= OnAttackCanceled;
        reloadAction.performed -= OnReloadPerformed;

        selectWeapon1Action.performed -= OnSelectWeapon1Performed;
        selectWeapon2Action.performed -= OnSelectWeapon2Performed;
    }

    private void Update()
    {
        moveInput = moveAction.ReadValue<Vector2>();
        attackIsHeld = attackAction.ReadValue<float>() > .5f;

        mouseScrollInput = scrollWeaponAction.ReadValue<float>();
    }

    void LateUpdate()
    {
        dashWasPressed = false;
        attackWasPressed = false;
        attackWasReleased = false;
        reloadWasPressed = false;

        selectWeapon1WasPressed = false;
        selectWeapon2WasPressed = false;
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

    private void OnAttackCanceled(InputAction.CallbackContext context)
    {
        attackWasReleased = true;
    }

    private void OnReloadPerformed(InputAction.CallbackContext context)
    {
        reloadWasPressed = true;
    }

    private void OnSelectWeapon1Performed(InputAction.CallbackContext context)
    {
        selectWeapon1WasPressed = true;
    }

    private void OnSelectWeapon2Performed(InputAction.CallbackContext context)
    {
        selectWeapon2WasPressed = true;
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