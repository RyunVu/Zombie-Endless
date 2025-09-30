using System;
using UnityEngine;

[DisallowMultipleComponent]
public class WeaponPreFiveEvent : MonoBehaviour
{
    public event Action<WeaponPreFiveEvent, WeaponPreFireEventArgs> OnWeaponPreFireEvent;
    public void CallOnWeaponPreFireEvent(WeaponPreFiveEvent weaponPreFiveEvent, WeaponPreFireEventArgs weaponPreFiveEventArgs)
    {
        OnWeaponPreFireEvent?.Invoke(this, new WeaponPreFireEventArgs());
    }
}

public class WeaponPreFireEventArgs : EventArgs
{
    public Weapon weapon;
}