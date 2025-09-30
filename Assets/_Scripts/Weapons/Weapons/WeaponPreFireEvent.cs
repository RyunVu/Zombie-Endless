using System;
using UnityEngine;

[DisallowMultipleComponent]
public class WeaponPreFireEvent : MonoBehaviour
{
    public event Action<WeaponPreFireEvent, WeaponPreFireEventArgs> OnWeaponPreFireEvent;
    public void CallOnWeaponPreFireEvent(bool fire, AimDirection aimDirection, float aimAngle, float weaponAimAngle, Vector3 weaponAimDirectionVector)
    {
        OnWeaponPreFireEvent?.Invoke(this, new WeaponPreFireEventArgs()
        {
            fire = fire,
            aimDirection = aimDirection,
            aimAngle = aimAngle,
            weaponAimAngle = weaponAimAngle,
            weaponAimDirectionVector = weaponAimDirectionVector    
        });
    }
}

public class WeaponPreFireEventArgs : EventArgs
{
    public bool fire;
    public AimDirection aimDirection;
    public float aimAngle;
    public float weaponAimAngle;
    public Vector3 weaponAimDirectionVector;
}