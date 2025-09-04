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

        _player.idleEvent.OnIdle += IdleEvent_OnIdle;
    }

    void OnDisable()
    {
        _player.movementByVelocityEvent.OnMovementByVelocity -= MovementByVelocityEvent_OnMovementByVelocity;

        _player.idleEvent.OnIdle -= IdleEvent_OnIdle;
    }

    private void MovementByVelocityEvent_OnMovementByVelocity(MovementByVelocityEvent movementByVelocityEvent, MovementByVelocityArgs movementByVelocityArgs)
    {
        SetMovementAnimationParemeters();
    }

    private void IdleEvent_OnIdle(IdleEvent idleEvent)
    {
        SetIdleAnimationParameters();
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
}