using System;
using UnityEngine;

public class WeaponFiredEvent : MonoBehaviour
{
    public event Action<WeaponFiredEvent, WeaponFiredEventArgs> OnWeaponFiredEvent;

    public void CallOnWeaponFiredEvent(Weapon weapon)
    {
        OnWeaponFiredEvent?.Invoke(this, new WeaponFiredEventArgs() { weapon = weapon });
    }
}

public class WeaponFiredEventArgs : EventArgs
{
    public Weapon weapon;
}