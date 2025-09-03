using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(PlayerControl))]
[RequireComponent(typeof(MovementByVelocityEvent))]
[RequireComponent(typeof(MovementByVelocity))]
public class Player : MonoBehaviour
{
    [HideInInspector] public PlayerInput playerInput;
    [HideInInspector] public PlayerControl playerControl;
    [HideInInspector] public MovementByVelocity movementByVelocity;
    [HideInInspector] public MovementByVelocityEvent movementByVelocityEvent;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerControl = GetComponent<PlayerControl>();
        movementByVelocity = GetComponent<MovementByVelocity>();
        movementByVelocityEvent = GetComponent<MovementByVelocityEvent>();
    }
}
