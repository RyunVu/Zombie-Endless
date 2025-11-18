using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(ReloadWeaponEvent))]
[RequireComponent(typeof(WeaponReloadedEvent))]
[RequireComponent(typeof(SetActiveWeaponEvent))]
[DisallowMultipleComponent]
public class ReloadWeapon : MonoBehaviour
{
    private ReloadWeaponEvent _reloadWeaponEvent;
    private WeaponReloadedEvent _weaponReloadedEvent;
    private SetActiveWeaponEvent _setActiveWeaponEvent;
    private Coroutine _reloadWeaponCoroutine;

    private void Awake()
    {
        _reloadWeaponEvent = GetComponent<ReloadWeaponEvent>();
        _weaponReloadedEvent = GetComponent<WeaponReloadedEvent>();
        _setActiveWeaponEvent = GetComponent<SetActiveWeaponEvent>();
    }

    private void OnEnable()
    {
        _reloadWeaponEvent.OnReloadWeapon += ReloadWeaponEvent_OnReloadWeapon;
        _setActiveWeaponEvent.OnSetActiveWeapon += SetActiveWeaponEvent_OnSetActiveWeapon;
    }

    private void OnDisable()
    {
        _reloadWeaponEvent.OnReloadWeapon -= ReloadWeaponEvent_OnReloadWeapon;
        _setActiveWeaponEvent.OnSetActiveWeapon -= SetActiveWeaponEvent_OnSetActiveWeapon;
    }

    private void ReloadWeaponEvent_OnReloadWeapon(ReloadWeaponEvent @event, ReloadWeaponArgs args)
    {
        StartReloadWeapon(args);
    }


    private void StartReloadWeapon(ReloadWeaponArgs args)
    {
        if (_reloadWeaponCoroutine != null)
            StopCoroutine(_reloadWeaponCoroutine);

        _reloadWeaponCoroutine = StartCoroutine(ReloadWeaponRoutine(args.weapon, args.topUpAmmoPercent));
    }

    private IEnumerator ReloadWeaponRoutine(Weapon weapon, int topUpAmmoPercent)
    {
        WeaponDetailsSO details = weapon.weaponDetails;

        if (details is not RangedWeaponDetailsSO ranged) yield break;

        weapon.isWeaponReloading = true;

        // Reload progess time
        while (weapon.weaponReloadTimer < ranged.weaponReloadTime)
        {
            weapon.weaponReloadTimer += Time.deltaTime;
            yield return null;
        }

        // Total ammo update
        if (topUpAmmoPercent != 0)
        {
            int ammoIncrease = Mathf.RoundToInt((ranged.weaponAmmoCapacity * topUpAmmoPercent) / 100f);

            int totalAmmo = weapon.weaponTotalAmmoRemaining + ammoIncrease;

            if (totalAmmo > ranged.weaponAmmoCapacity)
                weapon.weaponTotalAmmoRemaining = ranged.weaponAmmoCapacity;
            else
                weapon.weaponTotalAmmoRemaining = totalAmmo;
        }

        // Has infinite ammo
        if (ranged.hasInfiniteAmmo)
            weapon.weaponClipAmmoRemaining = ranged.weaponClipAmmoCapacity;

        else
        {
            int neededAmmo = ranged.weaponClipAmmoCapacity - weapon.weaponClipAmmoRemaining;
            int ammoToLoad = Mathf.Min(neededAmmo, weapon.weaponTotalAmmoRemaining);

            weapon.weaponClipAmmoRemaining += ammoToLoad;
            weapon.weaponTotalAmmoRemaining -= ammoToLoad;
        }

        // Finish reload --> Reset flag and Call weapon reloaded event

        weapon.weaponReloadTimer = 0f;

        weapon.isWeaponReloading = false;

        _weaponReloadedEvent.CallWeaponReloadedEvent(weapon);
    }
    
    // Restart the reload coroutine when a weapon that was mid-reload is switched away from and then re-equipped
    private void SetActiveWeaponEvent_OnSetActiveWeapon(SetActiveWeaponEvent @event, SetActiveWeaponEventArgs args)
    {
        if (args.weapon.isWeaponReloading)
        {
            if (_reloadWeaponCoroutine != null)
            {
                StopCoroutine(_reloadWeaponCoroutine);
            }

            _reloadWeaponCoroutine = StartCoroutine(ReloadWeaponRoutine(args.weapon, 0));
        }
    }
}