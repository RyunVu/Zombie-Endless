using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(WeaponPreFireEvent))]
public class WeaponPreFire : MonoBehaviour
{
    private WeaponPreFireEvent _weaponPreFireEvent;

    void Awake()
    {
        _weaponPreFireEvent = GetComponent<WeaponPreFireEvent>();
    }
}