using UnityEngine;

[CreateAssetMenu(fileName = "WeaponDetail_", menuName = "Scriptable Object/Weapons/Weapon Details")]
public class WeaponDetailsSO : ScriptableObject
{
    #region Header WEAPON BASE DETAILS
    [Space(10)]
    [Header("WEAPON BASE DETAILS")]
    #endregion Header WEAPON BASE DETAILS 
    [Tooltip("Weapon name")]    
    public string weaponName;
    
    [Tooltip("The sprite for the weapon - the sprite should have the 'generate physics shape' option selected ")]
    public Sprite weaponSprite;
    
    [Tooltip("Melee or Ranged")]
    public WeaponType weaponType;

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

    #region Header MELEE SETTINGS
    [Space(10)]
    #region Header MELEE WEAPON CONFIGURATION
    [Header("MELEE SETTINGS")]
    #endregion
    [Tooltip("Damage per melee hit")]
    public int meleeDamage = 10;

    [Tooltip("How far the attack reaches")]
    public float meleeRange = 1.5f;

    [Tooltip("Attack swing angel (degrees)")]
    public float meleeArc = 90f;

    [Tooltip("How long does the swing takes")]
    public float meleeSwingDuration = .2f;

    [Tooltip("Cooldown before next swing")]
    public float meleeCooldown = .4f;

    [Tooltip("Knockback force appiled to enemies")]
    public float meleeKnockback = 5f;

    [Tooltip("Can hit multiple enemies?")]
    public bool meleeCanPierce = true;

    #endregion

}