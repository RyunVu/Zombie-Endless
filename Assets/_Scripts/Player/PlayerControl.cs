using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.InputSystem;

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
        AimInput(out float playerAngleDegrees, out AimDirection playerAimDirection);
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

    private void AimInput(out float playerAngleDegrees, out AimDirection playerAimDirection)
    {
        Vector3 mouseWorldPositon = HelperUtilities.GetMouseWorldPosition();

        Vector3 playerDirection = (mouseWorldPositon - transform.position);

        playerAngleDegrees = HelperUtilities.GetAngleFromVector(playerDirection);

        playerAimDirection = HelperUtilities.GetAimDirection(playerAngleDegrees);

        switch (playerAimDirection)
        {
            case AimDirection.Left:
                _player.transform.localScale = new Vector3(-1f, 1f, 0f);
                break;

            case AimDirection.Right:
                _player.transform.localScale = new Vector3(1f, 1f, 0f);
                break;
        }
    }

}