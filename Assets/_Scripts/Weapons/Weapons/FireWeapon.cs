using System;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(ActiveWeapon))]
[RequireComponent(typeof(FireWeaponEvent))]
[RequireComponent(typeof(WeaponFiredEvent))]
[RequireComponent(typeof(ReloadWeaponEvent))]
public class FireWeapon : MonoBehaviour
{
    private float _fireRateCooldownTimer = 0f;
    private ActiveWeapon _activeWeapon;
    private FireWeaponEvent _fireWeaponEvent;
    private WeaponFiredEvent _weaponFiredEvent;
    private ReloadWeaponEvent _reloadWeaponEvent;

    void Awake()
    {
        _activeWeapon = GetComponent<ActiveWeapon>();
        _fireWeaponEvent = GetComponent<FireWeaponEvent>();
        _weaponFiredEvent = GetComponent<WeaponFiredEvent>();
        _reloadWeaponEvent = GetComponent<ReloadWeaponEvent>();
    }

    void OnEnable()
    {
        _fireWeaponEvent.OnFireWeaponEvent += FireWeaponEvent_OnFireWeapon;
        _weaponFiredEvent.OnWeaponFiredEvent += WeaponFiredEvent_OnWeaponFired;
    }

    void OnDisable()
    {
        _fireWeaponEvent.OnFireWeaponEvent -= FireWeaponEvent_OnFireWeapon;
        _weaponFiredEvent.OnWeaponFiredEvent -= WeaponFiredEvent_OnWeaponFired;
    }


    void Update()
    {
        _fireRateCooldownTimer -= Time.deltaTime;
    }

    private void FireWeaponEvent_OnFireWeapon(FireWeaponEvent @event, FireWeaponEventArgs args)
    {
        WeaponFire(args);
    }

    private void WeaponFiredEvent_OnWeaponFired(WeaponFiredEvent @event, WeaponFiredEventArgs args)
    {
        // Effect and other suff
    }


    private void WeaponFire(FireWeaponEventArgs args)
    {
        // Weapon Fire
        if (args.fire)
        {
            // Test if weapon is ready to 
            if (IsWeaponReadyToFire())
            {
                FireAmmo(args.aimAngle, args.weaponAimAngle, args.weaponAimDirectionVector);

                ResetFireRateCooldownTimer();
            }
        }
    }

    private bool IsWeaponReadyToFire()
    {
        
        WeaponDetailsSO details = _activeWeapon.GetCurrentWeapon().weaponDetails;
        if (details is not RangedWeaponDetailsSO ranged) return false;
        // No total ammo and dont have infinite ammo checked
        if (_activeWeapon.GetCurrentWeapon().weaponTotalAmmoRemaining <= 0 && !ranged.hasInfiniteAmmo)
            return false;

        // No ammo in the clip and dont have infinite ammo checked --> Call the Reload Weapon Event
        if (_activeWeapon.GetCurrentWeapon().weaponClipAmmoRemaining <= 0 && !ranged.hasInfiniteClipCapacity)
        {
            _reloadWeaponEvent.CallReloadWeaponEvent(_activeWeapon.GetCurrentWeapon(), 0);
            return false;
        }

        // The weapon reloading state
        if (_activeWeapon.GetCurrentWeapon().isWeaponReloading)
            return false;

        // On cool down timer
        if (_fireRateCooldownTimer > 0f)
            return false;

        // Ready to fire
        return true;

    }

    private void FireAmmo(float aimAngle, float weaponAimAngle, Vector3 weaponAimDirectionVector)
    {
        
        WeaponDetailsSO details = _activeWeapon.GetCurrentWeapon().weaponDetails;
        if (details is not RangedWeaponDetailsSO ranged) return;

        AmmoDetailsSO ammoDetail = _activeWeapon.GetCurrentAmmo();

        if (ammoDetail != null)
        {
            // Get ammo from array
            GameObject ammoPrefab = ammoDetail.ammoPrefabArray[UnityEngine.Random.Range(0, ammoDetail.ammoPrefabArray.Length)];

            // Get random speed value
            float ammoSpeed = UnityEngine.Random.Range(ammoDetail.ammoSpeedMin, ammoDetail.ammoSpeedMax);

            // Get ammo from the IFireable component pool
            IFireable ammo = (IFireable)PoolManager.Instance.ReuseComponent(ammoPrefab, _activeWeapon.GetShootPosition(), Quaternion.identity);

            ammo.InitialiseAmmo(ammoDetail, aimAngle, weaponAimAngle, ammoSpeed, weaponAimDirectionVector);

            // Reduces the ammo in the clip
            if (!ranged.hasInfiniteClipCapacity)
            {
                _activeWeapon.GetCurrentWeapon().weaponClipAmmoRemaining--;
                _activeWeapon.GetCurrentWeapon().weaponTotalAmmoRemaining--;
            }

            // Call the weapon fired event
            _weaponFiredEvent.CallWeaponFiredEvent(_activeWeapon.GetCurrentWeapon());
        }
    }

    private void ResetFireRateCooldownTimer()
    {
        WeaponDetailsSO details = _activeWeapon.GetCurrentWeapon().weaponDetails;
        if (details is not RangedWeaponDetailsSO ranged) return;
        _fireRateCooldownTimer = ranged.weaponFireRate;
    }
}