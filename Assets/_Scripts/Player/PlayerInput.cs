using UnityEngine;
using UnityEngine.InputSystem;

[DisallowMultipleComponent]
public class PlayerInput : SingletonMonobehaviour<PlayerInput>
{

    [Header("Input Actions Asset")]
    [SerializeField] private InputActionAsset inputActions;

    // Input Action
    public InputAction _moveAction;

    // Input Value
    public Vector2 moveInput { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        var actionMap = inputActions.FindActionMap("Player");

        _moveAction = actionMap.FindAction("Movement");
    }

    private void OnEnable()
    {
        _moveAction.Enable();
    }

    private void OnDisable()
    {
        _moveAction.Disable();
    }

    private void Update()
    {
        moveInput = _moveAction.ReadValue<Vector2>();
    }

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