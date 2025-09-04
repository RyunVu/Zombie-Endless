using UnityEngine;

[DisallowMultipleComponent]
public class PlayerControl : MonoBehaviour
{
    #region Tooltip

    [Tooltip("MovementDetailsSO scriptable object containing movement details such as speed")]

    #endregion Tooltip

    [SerializeField] private MovementDetailsSO movementDetails;

    private Player _player;
    private float _moveSpeed;

    private void Awake()
    {
        _player = GetComponent<Player>();

        _moveSpeed = movementDetails.GetMoveSpeed();
    }

    void FixedUpdate()
    {
        MovementInput();
    }

    private void MovementInput()
    {
        Vector2 moveInput = PlayerInput.Instance.moveInput;

        float horizontalMovement = moveInput.x;
        float verticalMovement = moveInput.y;

        Vector2 direction = new Vector2(horizontalMovement, verticalMovement);

        // Adjust distance for diagonal movement (pythagoras approximation)
        if (horizontalMovement != 0f && verticalMovement != 0f)
        {
            direction = direction.normalized;
        }

        if (direction != Vector2.zero)
        {
            _player.movementByVelocityEvent.CallMovementByVelocityEvent(direction, _moveSpeed);
        }
        else
        {
            _player.idleEvent.CallIdleEvent();
        }
    }
}