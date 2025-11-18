using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SubWeaponStatusUI : MonoBehaviour {
    [SerializeField] private Image _frameImage;
    [SerializeField] private Image _weaponImage;
    [SerializeField] private TextMeshProUGUI _ammoRemainingText;
    [SerializeField] private Sprite _noWeaponSprite;

    private Player _player;

    private Weapon _subWeapon;

    private void Awake()
    {
        _player = GameManager.Instance.GetPlayer();
    }

    void OnEnable()
    {
        _player.setActiveWeaponEvent.OnSetActiveWeapon += SetWeaponEvent_OnSetActiveWeapon;
    }

    void OnDisable()
    {
        _player.setActiveWeaponEvent.OnSetActiveWeapon -= SetWeaponEvent_OnSetActiveWeapon;
    }

    private void SetWeaponEvent_OnSetActiveWeapon(SetActiveWeaponEvent @event, SetActiveWeaponEventArgs args)
    {
        Weapon active = _player.GetMainWeapon();
        Weapon sub = _player.GetSubWeapon();

        if (sub == active) sub = null;

        UpdateSubWeaponStatusUI(sub);
    }

    private void UpdateSubWeaponStatusUI(Weapon weapon)
    {
        _subWeapon = weapon;

        // Case 1: Player has no secondary weapon
        if (_subWeapon == null)
        {
            _weaponImage.sprite = _noWeaponSprite;
            _ammoRemainingText.text = "";
        }

        // Case 2: Player has a secondary weapon
        else
        {
            Debug.Log("_subWeapon: " + _subWeapon.weaponDetails.weaponName);
            _weaponImage.sprite = _subWeapon.weaponDetails.weaponSprite;
            UpdateAmmoText(weapon);
        }
    }

    private void UpdateAmmoText(Weapon weapon)
    {       
        // Handle RangedWeapon
        if (weapon.weaponDetails is RangedWeaponDetailsSO ranged)
        {
            _ammoRemainingText.text = ranged.hasInfiniteAmmo
                ? "INFINITE AMMO"
                : $"{weapon.weaponClipAmmoRemaining} / {weapon.weaponTotalAmmoRemaining}";
        }
        // Handle MeleeWeapon
        else if (weapon.weaponDetails is MeleeWeaponDetailsSO)
        {
            _ammoRemainingText.text = ""; // Melee weapons have no ammo
        }
        else
        {
            _ammoRemainingText.text = ""; // fallback for any other weapon types
        }
    }
    

}