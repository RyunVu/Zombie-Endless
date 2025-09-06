using System;
using System.ComponentModel;
using UnityEngine;

[RequireComponent(typeof(Player))]
[DisallowMultipleComponent]
public class AnimatePlayer : MonoBehaviour
{
    #region ANIMATOR STATES
    public const string PLAYER_IDLE = "Idle";
    public const string PLAYER_RUN = "Run";
    public const string PLAYER_DASH = "Dash";
    #endregion
    private Player _player;

    void Awake()
    {
        _player = GetComponent<Player>();
    }

    void OnEnable()
    {
        _player.movementByVelocityEvent.OnMovementByVelocity += MovementByVelocityEvent_OnMovementByVelocity;

        _player.movementToPositionEvent.OnMovementToPosition += MovementByVelocityEvent_OnMovementByVelocity;

        _player.idleEvent.OnIdle += IdleEvent_OnIdle;
    }

    void OnDisable()
    {
        _player.movementByVelocityEvent.OnMovementByVelocity -= MovementByVelocityEvent_OnMovementByVelocity;
        _player.movementToPositionEvent.OnMovementToPosition -= MovementByVelocityEvent_OnMovementByVelocity;

        _player.idleEvent.OnIdle -= IdleEvent_OnIdle;
    }


    // Handle dash movement
    private void MovementByVelocityEvent_OnMovementByVelocity(MovementToPositionEvent @event, MovementToPositionArgs args)
    {
        InitializeDashAnimationParameters();
        SetMovementToPositionAnimationParameters(args);
    }


    // Handle regular movement (walking)
    private void MovementByVelocityEvent_OnMovementByVelocity(MovementByVelocityEvent movementByVelocityEvent, MovementByVelocityArgs movementByVelocityArgs)
    {
        SetMovementAnimationParemeters();
    }

    private void IdleEvent_OnIdle(IdleEvent idleEvent)
    {
        SetIdleAnimationParameters();
    }

    private void InitializeDashAnimationParameters()
    {
        _player.animator.SetBool(Settings.isDashing, false);
    }

    private void SetIdleAnimationParameters()
    {
        _player.animator.SetBool(Settings.isMoving, false);
        _player.animator.SetBool(Settings.isIdle, true);
        _player.animator.SetBool(Settings.isDashing, false);
    }

    private void SetMovementAnimationParemeters()
    {
        _player.animator.SetBool(Settings.isMoving, true);
        _player.animator.SetBool(Settings.isIdle, false);
        _player.animator.SetBool(Settings.isDashing, false);
    }

    
    private void SetMovementToPositionAnimationParameters(MovementToPositionArgs args)
    {
        if (args.isDashing)
        {
            _player.animator.SetBool(Settings.isDashing, true);
            _player.animator.SetBool(Settings.isMoving, false);
            _player.animator.SetBool(Settings.isIdle, false);
        }
    }
}