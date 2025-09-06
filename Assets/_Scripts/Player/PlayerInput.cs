using UnityEngine;
using UnityEngine.InputSystem;

[DisallowMultipleComponent]
public class PlayerInput : SingletonMonobehaviour<PlayerInput>
{

    [Header("Input Actions Asset")]
    [SerializeField] private InputActionAsset inputActions;

    // Input Action
    public InputAction _moveAction;

    public InputAction _dashAction;

    // Input Value
    public Vector2 moveInput { get; private set; }
    public bool dashWasPressed { get; private set; }


    protected override void Awake()
    {
        base.Awake();

        var actionMap = inputActions.FindActionMap("Player");

        _moveAction = actionMap.FindAction("Movement");
        _dashAction = actionMap.FindAction("Dash");
    }

    private void OnEnable()
    {
        _moveAction.Enable();
        _dashAction.Enable();

        _dashAction.performed += OnDashPerformed;
    }

    private void OnDisable()
    {
        _moveAction.Disable();
        _dashAction.Disable();

        _dashAction.performed -= OnDashPerformed;
    }

    private void Update()
    {
        moveInput = _moveAction.ReadValue<Vector2>();
    }

    void LateUpdate()
    {
        dashWasPressed = false;
    }

    #region Input events handler

    private void OnDashPerformed(InputAction.CallbackContext context)
    {
        dashWasPressed = true;
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