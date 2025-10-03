using System;
using System.Collections.Generic;
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
[RequireComponent(typeof(MovementToPosition))]
[RequireComponent(typeof(MovementToPositionEvent))]
[RequireComponent(typeof(Health))]

[RequireComponent(typeof(SetActiveWeaponEvent))]
[RequireComponent(typeof(ActiveWeapon))]
[RequireComponent(typeof(AimWeapon))]
[RequireComponent(typeof(AimWeaponEvent))]
[RequireComponent(typeof(WeaponFiredEvent))]
[RequireComponent(typeof(FireWeaponEvent))]
[RequireComponent(typeof(FireWeapon))]
[RequireComponent(typeof(ReloadWeaponEvent))]
[RequireComponent(typeof(WeaponReloadedEvent))]
[RequireComponent(typeof(ReloadWeapon))]

public class Player : MonoBehaviour
{
    [HideInInspector] public PlayerDetailsSO playerDetailsSO;
    [HideInInspector] public PlayerInput playerInput;
    [HideInInspector] public PlayerControl playerControl;
    [HideInInspector] public MovementByVelocity movementByVelocity;
    [HideInInspector] public MovementByVelocityEvent movementByVelocityEvent;
    [HideInInspector] public Idle idle;
    [HideInInspector] public IdleEvent idleEvent;
    [HideInInspector] public MovementToPosition movementToPosition;
    [HideInInspector] public MovementToPositionEvent movementToPositionEvent;
    [HideInInspector] public Health health;
    [HideInInspector] public SpriteRenderer spriteRenderer;
    [HideInInspector] public Animator animator;
    [HideInInspector] public AnimatePlayer animatePlayer;
    [HideInInspector] public ActiveWeapon activeWeapon;
    [HideInInspector] public SetActiveWeaponEvent setActiveWeaponEvent;
    [HideInInspector] public AimWeapon aimWeapon;
    [HideInInspector] public AimWeaponEvent aimWeaponEvent;
    [HideInInspector] public FireWeaponEvent fireWeaponEvent;
    [HideInInspector] public WeaponFiredEvent weaponFiredEvent;
    [HideInInspector] public FireWeapon fireWeapon;
    [HideInInspector] public ReloadWeaponEvent reloadWeaponEvent;
    [HideInInspector] public WeaponReloadedEvent weaponReloadedEvent;
    [HideInInspector] public ReloadWeapon reloadWeapon;

    public List<Weapon> weaponList = new();

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
        movementToPosition = GetComponent<MovementToPosition>();
        movementToPositionEvent = GetComponent<MovementToPositionEvent>();
        health = GetComponent<Health>();
        animatePlayer = GetComponent<AnimatePlayer>();

        activeWeapon = GetComponent<ActiveWeapon>();
        setActiveWeaponEvent = GetComponent<SetActiveWeaponEvent>();
        aimWeapon = GetComponent<AimWeapon>();
        aimWeaponEvent = GetComponent<AimWeaponEvent>();
        fireWeaponEvent = GetComponent<FireWeaponEvent>();
        weaponFiredEvent = GetComponent<WeaponFiredEvent>();
        fireWeapon = GetComponent<FireWeapon>();
        reloadWeaponEvent = GetComponent<ReloadWeaponEvent>();
        weaponReloadedEvent = GetComponent<WeaponReloadedEvent>();
        reloadWeapon = GetComponent<ReloadWeapon>();

    }

    public void Initialize(PlayerDetailsSO playerDetails)
    {
        this.playerDetailsSO = playerDetails;

        CreatePlayerStartingWeapon();

        SetPlayerHealth();
    }

    private void CreatePlayerStartingWeapon()
    {
        weaponList.Clear();

        foreach (WeaponDetailsSO weapon in playerDetailsSO.startingWeaponList)
        {
            AddWeaponToList(weapon);
        }

    }

    /// <summary>
    /// Add and equip the new weapon to the weapon dictionary
    /// </summary>
    private void AddWeaponToList(WeaponDetailsSO weaponDetail)
    {
        Weapon weapon = new Weapon()
        {
            weaponDetails = weaponDetail,
            weaponReloadTimer = 0f,
            weaponClipAmmoRemaining = weaponDetail.weaponClipAmmoCapacity,
            weaponTotalAmmoRemaining = weaponDetail.weaponAmmoCapacity,
            isWeaponReloading = false
        };

        weaponList.Add(weapon);
        weapon.weaponPositionInList = weaponList.Count;
        setActiveWeaponEvent.CallSetActiveWeaponEvent(weapon);
    }   

    private void SetPlayerHealth()
    {
        health.SetStartingHealth(playerDetailsSO.playerHealthAmount);
    }
}
