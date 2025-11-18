using UnityEngine;

[CreateAssetMenu(fileName = "RangedWeaponDetail_", menuName = "Scriptable Object/Weapons/Ranged Weapon Details")]
public class RangedWeaponDetailsSO : WeaponDetailsSO
{
    #region Header RANGED SETTINGS
    [Space(10)]
    #region Header RANGED WEAPON CONFIGURATION
    [Header("RANGED SETTINGS")]
    #endregion
    
    [Tooltip("Weapon Shoot Position - the offset position for the end of the weapon from the sprite pivot pont")]
    public Vector3 weaponShootPosition;

    [Tooltip("Weapon current ammo")]   
    public AmmoDetailsSO weaponCurrentAmmo;
    
    [Tooltip("Select if the weapon has infinite ammo")]
    public bool hasInfiniteAmmo = false;
 
    [Tooltip("Select if the weapon has infinite clip capacity")]
    public bool hasInfiniteClipCapacity = false;

    [Tooltip("The weapon capacity - shots before a reload")]
    public int weaponClipAmmoCapacity = 6;
    
    [Tooltip("Weapon ammo capacity - the maximum number of rounds at that can be held for this weapon")]
    public int weaponAmmoCapacity = 100;

    [Tooltip("Weapon Fire Rate - 0.2 means 5 shots a second")]
    public float weaponFireRate = 0.2f;

    [Tooltip("Weapon Precharge Time - time in seconds to hold fire button down before firing")]
    public float weaponPrechargeTime = 0f;

    [Tooltip("This is the weapon reload time in seconds")]
    public float weaponReloadTime = 0f;
    #endregion


}