using UnityEngine;

public abstract class WeaponDetailsSO : ScriptableObject
{
    [Header("Weapon Base Details")]
    public string weaponName;
    public Sprite weaponSprite;

    [Tooltip("Melee or Ranged")]
    public WeaponType weaponType;
}