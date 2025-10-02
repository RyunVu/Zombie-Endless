using System;
using UnityEngine;

[Serializable]
public class Weapon
{
    public WeaponDetailsSO weaponDetails;
    public int weaponPositionInList;
    public float weaponReloadTimer;
    public int weaponClipAmmoRemaining;
    public int weaponTotalAmmoRemaining;
    public bool isWeaponReloading;

}