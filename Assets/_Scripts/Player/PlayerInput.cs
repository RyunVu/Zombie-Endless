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

    public InputAction changeWeaponAction;

    public InputAction interactAction;

    // Input Value
    public Vector2 moveInput { get; private set; }
    public bool dashWasPressed { get; private set; }
    public bool attackWasPressed { get; private set; }
    public bool attackIsHeld { get; private set; }
    public bool attackWasReleased { get; private set; }
    public bool reloadWasPressed { get; private set; }

    public float mouseScrollInput { get; private set; }
    public bool changeWeaponPressed { get; private set; }

    public bool interactWasPressed { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        var actionMap = inputActions.FindActionMap("Player");

        moveAction = actionMap.FindAction("Movement");
        dashAction = actionMap.FindAction("Dash");
        attackAction = actionMap.FindAction("Attack");
        reloadAction = actionMap.FindAction("Reload");

        scrollWeaponAction = actionMap.FindAction("ScrollWeapon");
        changeWeaponAction = actionMap.FindAction("ChangeWeapon");

        interactAction = actionMap.FindAction("Interact");
    }

    private void OnEnable()
    {
        moveAction.Enable();
        dashAction.Enable();
        attackAction.Enable();
        reloadAction.Enable();

        scrollWeaponAction.Enable();
        changeWeaponAction.Enable();

        interactAction.Enable();

        dashAction.performed += OnDashPerformed;
        attackAction.performed += OnAttackPerformed;
        attackAction.canceled += OnAttackCanceled;
        reloadAction.performed += OnReloadPerformed;

        changeWeaponAction.performed += OnChangeWeaponPerformed;

        interactAction.performed += OnInteractPerformed;

    }

    private void OnDisable()
    {
        moveAction.Disable();
        dashAction.Disable();
        attackAction.Disable();
        reloadAction.Disable();

        scrollWeaponAction.Disable();
        changeWeaponAction.Disable();

        interactAction.Disable();

        dashAction.performed -= OnDashPerformed;
        attackAction.performed -= OnAttackPerformed;
        attackAction.canceled -= OnAttackCanceled;
        reloadAction.performed -= OnReloadPerformed;

        changeWeaponAction.performed -= OnChangeWeaponPerformed;

        interactAction.performed -= OnInteractPerformed;
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

        changeWeaponPressed = false;

        interactWasPressed = false;
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

    private void OnChangeWeaponPerformed(InputAction.CallbackContext context)
    {
        changeWeaponPressed = true;
    }

    private void OnInteractPerformed(InputAction.CallbackContext context)
    {
        interactWasPressed = true;
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