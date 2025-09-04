using System.Data.Common;
using UnityEngine;
using UnityEngine.Rendering;

[DisallowMultipleComponent]
[RequireComponent(typeof(SortingGroup))]            // Handle multiple sprite component
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(BoxCollider2D))]           // None trigger collider for block wall if added
[RequireComponent(typeof(PolygonCollider2D))]       // Trigger collider for ammo and being hit by enemy
[RequireComponent(typeof(Rigidbody2D))]

[RequireComponent(typeof(AnimatePlayer))]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(PlayerControl))]
[RequireComponent(typeof(MovementByVelocityEvent))]
[RequireComponent(typeof(MovementByVelocity))]
[RequireComponent(typeof(Idle))]
[RequireComponent(typeof(IdleEvent))]
[RequireComponent(typeof(Health))]
public class Player : MonoBehaviour
{
    [HideInInspector] public PlayerDetailsSO playerDetailsSO;
    [HideInInspector] public PlayerInput playerInput;
    [HideInInspector] public PlayerControl playerControl;
    [HideInInspector] public MovementByVelocity movementByVelocity;
    [HideInInspector] public MovementByVelocityEvent movementByVelocityEvent;
    [HideInInspector] public Idle idle;
    [HideInInspector] public IdleEvent idleEvent;
    [HideInInspector] public Health health;
    [HideInInspector] public SpriteRenderer spriteRenderer;
    [HideInInspector] public Animator animator;
    [HideInInspector] public AnimatePlayer animatePlayer;


    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        playerInput = GetComponent<PlayerInput>();
        playerControl = GetComponent<PlayerControl>();
        movementByVelocity = GetComponent<MovementByVelocity>();
        movementByVelocityEvent = GetComponent<MovementByVelocityEvent>();
        idle = GetComponent<Idle>();
        idleEvent = GetComponent<IdleEvent>();
        health = GetComponent<Health>();
        animatePlayer = GetComponent<AnimatePlayer>();
    }

    public void Initialize(PlayerDetailsSO playerDetails)
    {
        this.playerDetailsSO = playerDetails;

        SetPlayerHealth();
    }

    private void SetPlayerHealth()
    {
        health.SetStartingHealth(playerDetailsSO.playerHealthAmount);
    }
}
