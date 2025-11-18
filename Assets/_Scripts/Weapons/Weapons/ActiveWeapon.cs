using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(SetActiveWeaponEvent))]
[DisallowMultipleComponent]
public class ActiveWeapon : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _weaponSpriteRenderer;
    [SerializeField] private PolygonCollider2D _weaponPolygonCollider2D;

    // Ranged weapon shoot position transform
    [SerializeField] private Transform _weaponShootPositionTransform;
    [SerializeField] private Transform _weaponEffectPositionTransform;

    // Melee weapon hit position transform
    [SerializeField] private Transform _meleeHitPositionTransform;

    private SetActiveWeaponEvent _setActiveWeaponEvent;
    private Weapon _currentWeapon;

    void Awake()
    {
        _setActiveWeaponEvent = GetComponent<SetActiveWeaponEvent>();
    }

    void OnEnable()
    {
        _setActiveWeaponEvent.OnSetActiveWeapon += SetActiveWeaponEvent_OnSetActiveWeapon;
    }

    void OnDisable()
    {
        _setActiveWeaponEvent.OnSetActiveWeapon -= SetActiveWeaponEvent_OnSetActiveWeapon;
    }

    private void SetActiveWeaponEvent_OnSetActiveWeapon(SetActiveWeaponEvent sender, SetActiveWeaponEventArgs eventArgs)
    {
        SetWeapon(eventArgs.weapon);
    }

    private void SetWeapon(Weapon weapon)
    {
        if (weapon == null) return;

        _currentWeapon = weapon;

        List<Weapon> weaponList = GameManager.Instance.GetPlayer().weaponList;

        _weaponSpriteRenderer.sprite = weapon.weaponDetails.weaponSprite;

        if (_weaponPolygonCollider2D != null && _weaponSpriteRenderer.sprite != null)
        {
            List<Vector2> spritePhysicsShapePointsList = new List<Vector2>();
            _weaponSpriteRenderer.sprite.GetPhysicsShape(0, spritePhysicsShapePointsList);

            _weaponPolygonCollider2D.points = spritePhysicsShapePointsList.ToArray();
        }

        WeaponDetailsSO details = weapon.weaponDetails;
        if (details is not RangedWeaponDetailsSO ranged) return;

        _weaponShootPositionTransform.localPosition = ranged.weaponShootPosition;
    }

    public AmmoDetailsSO GetCurrentAmmo()
    {
        
        WeaponDetailsSO details = _currentWeapon.weaponDetails;
        if (details is not RangedWeaponDetailsSO ranged) return null;
        return ranged.weaponCurrentAmmo;
    }
    
    public Weapon GetCurrentWeapon()
    {
        return _currentWeapon;
    }

    public Vector3 GetShootPosition()
    {
        return _weaponShootPositionTransform.position;
    }

    public Vector3 GetShootEffectPosition()
    {
        return _weaponEffectPositionTransform.position;
    }
    
    public Vector3 GetMeleeHitPosition()
    {
        if (_meleeHitPositionTransform == null)
            return transform.position;

        return _meleeHitPositionTransform.position;
    }
}