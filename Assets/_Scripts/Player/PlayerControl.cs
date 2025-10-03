using System.Collections;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Player))]
[DisallowMultipleComponent]
public class PlayerControl : MonoBehaviour
{
    #region Tooltip

    [Tooltip("MovementDetailsSO scriptable object containing movement details such as speed")]

    #endregion Tooltip

    [SerializeField] private MovementDetailsSO movementDetails;

    private Player _player;
    private SpriteRenderer _playerSpriteRenderer;
    private float _moveSpeed;
    private bool _leftMouseDownPreviousFrame = false;
    private int _currentWeaponIndex = 1;

    #region DASHING VARIABLES
    private Coroutine _playerDashCoroutine;
    private WaitForFixedUpdate _waitForFixedUpdate;
    private bool _isPlayerDashing;
    private float _playerDashCooldownTimer = 0f;

    #endregion

    private void Awake()
    {
        _player = GetComponent<Player>();
        _playerSpriteRenderer = GetComponent<SpriteRenderer>();

        _moveSpeed = movementDetails.GetMoveSpeed();
    }

    void Start()
    {
        _waitForFixedUpdate = new WaitForFixedUpdate();

        SetStartingWeapon();
    }

    void Update()
    {
        if (_isPlayerDashing) return;

        MovementInput();

        WeaponInput();

        PlayerDashCooldownTimer();

    }

    void FixedUpdate()
    {
    }

    private void SetStartingWeapon()
    {
        int index = 1;

        foreach (Weapon weapon in _player.weaponList)
        {
            if (weapon.weaponDetails == _player.playerDetailsSO.startingWeapon)
            {
                SetWeaponByIndex(index);
                break;
            }
            index++;
        }
    }

    private void SetWeaponByIndex(int weaponIndex)
    {
        if (weaponIndex - 1 < _player.weaponList.Count)
        {
            _currentWeaponIndex = weaponIndex;

            _player.setActiveWeaponEvent.CallSetActiveWeaponEvent(_player.weaponList[weaponIndex - 1]);
        }
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
        Debug.Log("Hit something: " + collision.gameObject.name);
        StopPlayerRollRoutine();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        // if in collision with something stop player roll coroutine
        Debug.Log("Hit something: " + collision.gameObject.name);
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


    private void WeaponInput()
    {
        Vector3 weaponDirection;
        float weaponAngleDegrees, playerAngleDegrees;
        AimDirection playerAimDirection;

        AimWeaponInput(out weaponDirection, out weaponAngleDegrees, out playerAngleDegrees, out playerAimDirection);

        FireWeaponInput(weaponDirection, weaponAngleDegrees, playerAngleDegrees, playerAimDirection);
    }

    private void AimWeaponInput(out Vector3 weaponDirection, out float weaponAngleDegrees, out float playerAngleDegrees, out AimDirection playerAimDirection)
    {
        Vector3 mouseWorldPosition = HelperUtilities.GetMouseWorldPosition();

        weaponDirection = (mouseWorldPosition - _player.activeWeapon.GetShootPosition());

        Vector3 playerDirection = (mouseWorldPosition - transform.position);

        weaponAngleDegrees = HelperUtilities.GetAngleFromVector(weaponDirection);

        playerAngleDegrees = HelperUtilities.GetAngleFromVector(playerDirection);

        playerAimDirection = HelperUtilities.GetAimDirection(playerAngleDegrees);

        _player.aimWeaponEvent.CallAimWeaponEvent(playerAimDirection, playerAngleDegrees, weaponAngleDegrees, weaponDirection);

    }


    private void FireWeaponInput(Vector3 weaponDirection, float weaponAngleDegrees, float playerAngleDegrees, AimDirection playerAimDirection)
    {
        if (InputManager.AttackWasPressed)
        {
            _player.fireWeaponEvent.CallFireWeaponEvent(
                true,
                _leftMouseDownPreviousFrame,
                playerAimDirection,
                playerAngleDegrees,
                weaponAngleDegrees,
                weaponDirection);
            _leftMouseDownPreviousFrame = true;
        }
        else if (InputManager.AttackIsHeld)
        {
            _player.fireWeaponEvent.CallFireWeaponEvent(
                true,
                _leftMouseDownPreviousFrame,
                playerAimDirection,
                playerAngleDegrees,
                weaponAngleDegrees,
                weaponDirection
            );

            _leftMouseDownPreviousFrame = true;
        }
        // Stop firing when released
        else if (InputManager.AttackWasReleased)
        {
            _player.fireWeaponEvent.CallFireWeaponEvent(
                false,
                _leftMouseDownPreviousFrame,
                playerAimDirection,
                playerAngleDegrees,
                weaponAngleDegrees,
                weaponDirection
            );

            _leftMouseDownPreviousFrame = false;
        }
        else
            _leftMouseDownPreviousFrame = false;
    }
}