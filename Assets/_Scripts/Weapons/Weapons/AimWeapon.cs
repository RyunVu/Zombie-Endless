using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(AimWeaponEvent))]
public class AimWeapon : MonoBehaviour
{
    [SerializeField] private Transform _weaponRotationPointTransform;

    private AimWeaponEvent _aimWeaponEvent;

    private void Awake()
    {
        _aimWeaponEvent = GetComponent<AimWeaponEvent>();
    }

    void OnEnable()
    {
        _aimWeaponEvent.OnWeaponAim += AimWeaponEvent_OnWeaponAim;
    }

    void OnDisable()
    {
        _aimWeaponEvent.OnWeaponAim -= AimWeaponEvent_OnWeaponAim;
    }

    private void AimWeaponEvent_OnWeaponAim(AimWeaponEvent aimWeaponEvent, AimWeaponEventArgs aimWeaponEventArgs)
    {
        Aim(aimWeaponEventArgs.aimDirection, aimWeaponEventArgs.aimAngle);
    }

    private void Aim(AimDirection aimDirection, float aimAngle)
    {
        _weaponRotationPointTransform.eulerAngles = new Vector3(0f, 0f, aimAngle);

        switch (aimDirection)
        {
            case AimDirection.Left:
                _weaponRotationPointTransform.localScale = new Vector3(1f, -1f, 0f);
                break;
            case AimDirection.Right:
                _weaponRotationPointTransform.localScale = new Vector3(1f, 1f, 0f);
                break;
        }
    }
}
