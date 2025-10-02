using System;
using System.ComponentModel;
using UnityEngine;

[RequireComponent(typeof(Player))]
[DisallowMultipleComponent]
public class AnimatePlayer : MonoBehaviour
{
    private Player _player;

    void Awake()
    {
        _player = GetComponent<Player>();
    }

    void OnEnable()
    {
        _player.movementByVelocityEvent.OnMovementByVelocity += MovementByVelocityEvent_OnMovementByVelocity;

        _player.movementToPositionEvent.OnMovementToPosition += MovementByVelocityEvent_OnMovementToPosition;

        _player.idleEvent.OnIdle += IdleEvent_OnIdle;

        _player.aimWeaponEvent.OnWeaponAim += AimWeaponEvent_OnWeaponAim;
    }

    void OnDisable()
    {
        _player.movementByVelocityEvent.OnMovementByVelocity -= MovementByVelocityEvent_OnMovementByVelocity;

        _player.movementToPositionEvent.OnMovementToPosition -= MovementByVelocityEvent_OnMovementToPosition;

        _player.idleEvent.OnIdle -= IdleEvent_OnIdle;

        _player.aimWeaponEvent.OnWeaponAim -= AimWeaponEvent_OnWeaponAim;

    }


    // Handle dash movement
    private void MovementByVelocityEvent_OnMovementByVelocity(MovementByVelocityEvent @event, MovementByVelocityArgs args)
    {
        InitializeDashAnimationParameters();
        SetMovementAnimationParemeters();
    }


    // Handle regular movement (walking)
    private void MovementByVelocityEvent_OnMovementToPosition(MovementToPositionEvent movementByVelocityEvent, MovementToPositionArgs movementToPositionArgs)
    {
        InitializeAimAnimationParameters();
        InitializeDashAnimationParameters();
        SetMovementToPositionAnimationParameters(movementToPositionArgs);
    }

    private void AimWeaponEvent_OnWeaponAim(AimWeaponEvent aimWeaponEvent, AimWeaponEventArgs aimWeaponEventArgs)
    {
        InitializeAimAnimationParameters();
        InitializeDashAnimationParameters();
        SetAimWeaponAnimationParameters(aimWeaponEventArgs.aimDirection);
    }

    private void IdleEvent_OnIdle(IdleEvent idleEvent)
    {
        InitializeDashAnimationParameters();
        SetIdleAnimationParameters();
    }

    private void InitializeAimAnimationParameters()
    {
        _player.animator.SetBool(Settings.aimLeft, false);
        _player.animator.SetBool(Settings.aimRight, false);
    }

    private void InitializeDashAnimationParameters()
    {
        _player.animator.SetBool(Settings.dashLeft, false);
        _player.animator.SetBool(Settings.dashRight, false);
    }

    private void SetIdleAnimationParameters()
    {
        _player.animator.SetBool(Settings.isMoving, false);
        _player.animator.SetBool(Settings.isIdle, true);
    }

    private void SetMovementAnimationParemeters()
    {
        _player.animator.SetBool(Settings.isMoving, true);
        _player.animator.SetBool(Settings.isIdle, false);
    }


    private void SetMovementToPositionAnimationParameters(MovementToPositionArgs args)
    {
        if (args.isDashing)
        {
            if (args.moveDirection.x > 0f)
                _player.animator.SetBool(Settings.dashRight, true);

            else if (args.moveDirection.x < 0f)
                _player.animator.SetBool(Settings.dashLeft, true);
                
        }
    }

    private void SetAimWeaponAnimationParameters(AimDirection aimDirection)
    {
        switch (aimDirection)
        {
            case AimDirection.Left:
                _player.animator.SetBool(Settings.aimLeft, true);
                break;
            case AimDirection.Right:
                _player.animator.SetBool(Settings.aimRight, true);
                break;
        }
    }
    
}