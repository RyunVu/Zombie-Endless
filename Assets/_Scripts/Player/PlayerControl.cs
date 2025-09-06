using System.Collections;
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

    #region DASHING VARIABLES
    private Coroutine _playerDashCoroutine;
    private WaitForFixedUpdate _waitForFixedUpdate;
    private bool _isPlayerDashing;
    private float _playerDashCooldownTimer = 0f;

    #endregion

    private void Awake()
    {
        _player = GetComponent<Player>();

        _moveSpeed = movementDetails.GetMoveSpeed();
    }

    void Start()
    {
        _waitForFixedUpdate = new WaitForFixedUpdate();

    }

    void Update()
    {
        if (_isPlayerDashing) return;

        MovementInput();

        PlayerDashCooldownTimer();

        AimInput(out float playerAngleDegrees, out AimDirection playerAimDirection);
    }

    void FixedUpdate()
    {
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
            if (!InputManager.DashWasPressed)
            {
                _player.movementByVelocityEvent.CallMovementByVelocityEvent(direction, _moveSpeed);
            }
            else if (_playerDashCooldownTimer <= 0f)
            {
                Debug.Log("Dash was pressed");
                PlayerDash((Vector3)direction);
            }
        }
        else
        {
            _player.idleEvent.CallIdleEvent();
        }
    }

    private void PlayerDash(Vector3 direction)
    {
        _playerDashCoroutine = StartCoroutine(PlayerDashCoroutine(direction));
    }

    private IEnumerator PlayerDashCoroutine(Vector3 direction)
    {
        // minDistance used to determine when to stop couroutine loop
        float minDistance = .2f;

        _isPlayerDashing = true;

        Vector3 targetPosition = _player.transform.position + (Vector3)direction * movementDetails.dashDistance;

        while (Vector3.Distance(_player.transform.position, targetPosition) > minDistance)
        {
            _player.movementToPositionEvent.CallMovementToPositionEvent(targetPosition, _player.transform.position, movementDetails.dashSpeed, direction, _isPlayerDashing);

            yield return _waitForFixedUpdate;
        }

        _isPlayerDashing = false;
        _playerDashCooldownTimer = movementDetails.dashCooldownTime;
        _player.transform.position = targetPosition;
    }

    private void PlayerDashCooldownTimer()
    {
        if (_playerDashCooldownTimer >= 0f)
        {
            _playerDashCooldownTimer -= Time.deltaTime;
        }
    }

     private void OnCollisionEnter2D(Collision2D collision)
    {
        // if collided with something stop player roll coroutine
        StopPlayerRollRoutine();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        // if in collision with something stop player roll coroutine
        StopPlayerRollRoutine();
    }

    private void StopPlayerRollRoutine()
    {
        if (_playerDashCoroutine != null)
        {
            StopCoroutine(_playerDashCoroutine);

            _isPlayerDashing = false;
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