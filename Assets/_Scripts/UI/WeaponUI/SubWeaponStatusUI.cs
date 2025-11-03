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
        UpdateSubWeaponStatusUI(_player.GetSubWeapon());
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
            _weaponImage.sprite = _subWeapon.weaponDetails.weaponSprite;
            UpdateAmmoText(_subWeapon);
        }
    }

    private void UpdateAmmoText(Weapon weapon)
    {
        if (weapon.weaponDetails.hasInfiniteAmmo)
            _ammoRemainingText.text = "INFINITE AMMO";
        else
            _ammoRemainingText.text = weapon.weaponClipAmmoRemaining.ToString() + " / " + weapon.weaponTotalAmmoRemaining.ToString();
    }
    

}