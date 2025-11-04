using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainWeaponStatusUI : MonoBehaviour
{
    [Tooltip("Reference to the Image component on the child 'WeaponImage' object — used to display the weapon’s icon in the UI.")]
    [SerializeField] private Image _weaponImage;

    [Tooltip("Reference to the Transform of the child 'AmmoHolder' object — acts as the parent container for all ammo-related UI elements.")]
    [SerializeField] private Transform _ammoHolderTransform;

    [Tooltip("Reference to the TextMeshProUGUI component on the child 'ReloadText' object — shows a message (e.g., 'Reloading...') when the weapon is reloading.")]
    [SerializeField] private TextMeshProUGUI _reloadText;

    [Tooltip("Reference to the TextMeshProUGUI component on the child 'AmmoRemainingText' object — displays the current ammo count for the weapon.")]
    [SerializeField] private TextMeshProUGUI _ammoRemainingText;

    [Tooltip("Reference to the TextMeshProUGUI component on the child 'WeaponNameText' object — displays the active weapon’s name.")]
    [SerializeField] private TextMeshProUGUI _weaponNameText;

    [Tooltip("Reference to the RectTransform of the child 'ReloadBar' object — used to visually represent the weapon’s reload progress.")]
    [SerializeField] private Transform _reloadBar;

    [Tooltip("Reference to the Image component on the child 'BarImage' object — fills to indicate reload progress visually.")]
    [SerializeField] private Image _barImage;

    private Player _player;
    private List<GameObject> ammoIconList = new();
    private Coroutine _reloadWeaponCoroutine;
    private Coroutine _blinkingReloadTextCoroutine;

    void Awake()
    {
        _player = GameManager.Instance.GetPlayer();
        Debug.Log("Player get init " + _player);
    }
    void Start()
    {
        // Update active weapon event on the UI
        SetActiveWeapon(_player.activeWeapon.GetCurrentWeapon());
    }

    void OnEnable()
    {
        _player.setActiveWeaponEvent.OnSetActiveWeapon += SetActiveWeaponEvent_OnSetActiveWeapon;
        _player.weaponFiredEvent.OnWeaponFiredEvent += WeaponFiredEvent_OnWeaponFiredEvent;
        _player.reloadWeaponEvent.OnReloadWeapon += ReloadWeaponEvent_OnReloadWeapon;
        _player.weaponReloadedEvent.OnWeaponReloaded += WeaponReloadedEvent_OnWeaponReloaded;

    }

    void OnDisable()
    {
        _player.setActiveWeaponEvent.OnSetActiveWeapon -= SetActiveWeaponEvent_OnSetActiveWeapon;
        _player.weaponFiredEvent.OnWeaponFiredEvent -= WeaponFiredEvent_OnWeaponFiredEvent;
        _player.reloadWeaponEvent.OnReloadWeapon -= ReloadWeaponEvent_OnReloadWeapon;
        _player.weaponReloadedEvent.OnWeaponReloaded -= WeaponReloadedEvent_OnWeaponReloaded;
    }

    private void SetActiveWeaponEvent_OnSetActiveWeapon(SetActiveWeaponEvent @event, SetActiveWeaponEventArgs args)
    {
        SetActiveWeapon(args.weapon);
    }

    private void WeaponFiredEvent_OnWeaponFiredEvent(WeaponFiredEvent @event, WeaponFiredEventArgs args)
    {
        WeaponFired(args.weapon);
    }

    private void WeaponFired(Weapon weapon)
    {
        UpdateAmmoText(weapon);
        UpdateAmmoLoadedIcons(weapon);
        UpdateReloadText(weapon);
    }

    private void ReloadWeaponEvent_OnReloadWeapon(ReloadWeaponEvent @event, ReloadWeaponArgs args)
    {
        UpdateWeaponReloadBar(args.weapon);
    }

    private void WeaponReloadedEvent_OnWeaponReloaded(WeaponReloadedEvent @event, WeaponReloadedArgs args)
    {
        WeaponReloaded(args.weapon);
    }

    private void WeaponReloaded(Weapon weapon)
    {
        if (_player.activeWeapon.GetCurrentWeapon() == weapon)
        {
            UpdateAmmoText(weapon);
            UpdateReloadText(weapon);
            UpdateAmmoLoadedIcons(weapon);
            ResetWeaponReloadBar();
        }
    }

    private void SetActiveWeapon(Weapon weapon)
    {
        UpdateActiveWeaponImage(weapon.weaponDetails);
        UpdateActiveWeaponName(weapon);
        UpdateAmmoText(weapon);
        UpdateAmmoLoadedIcons(weapon);

        if (weapon.isWeaponReloading)
            UpdateWeaponReloadBar(weapon);
        else
            ResetWeaponReloadBar();

        UpdateReloadText(weapon);
    }

    private void UpdateActiveWeaponImage(WeaponDetailsSO weaponDetails)
    {
        _weaponImage.sprite = weaponDetails.weaponSprite;
    }

    private void UpdateActiveWeaponName(Weapon weapon)
    {
        
        _weaponNameText.text = "(" + weapon.weaponPositionInList + ") " + weapon.weaponDetails.weaponName.ToUpper();
    }

    private void UpdateAmmoText(Weapon weapon)
    {
        if (weapon.weaponDetails.hasInfiniteAmmo)
            _ammoRemainingText.text = "INFINITE AMMO";
        else
            _ammoRemainingText.text = weapon.weaponClipAmmoRemaining.ToString() + " / " + weapon.weaponTotalAmmoRemaining.ToString();
    }

    private void UpdateAmmoLoadedIcons(Weapon weapon)
    {
        ClearAmmoLoadedIcons();

        for (int i = 0; i < weapon.weaponClipAmmoRemaining; i++)
        {
            GameObject ammoIcon = Instantiate(GameResources.Instance.ammoIconPrefab, _ammoHolderTransform);

            RectTransform ammoIconRectTransform = ammoIcon.GetComponent<RectTransform>();
            ammoIconRectTransform.anchoredPosition = new Vector2(0f, Settings.uiAmmoIconSpacing * i);

            ammoIconList.Add(ammoIcon);
        }
    }

    private void ClearAmmoLoadedIcons()
    {
        foreach (GameObject ammoIcon in ammoIconList)
        {
            Destroy(ammoIcon);
        }

        ammoIconList.Clear();
    }

    private void UpdateWeaponReloadBar(Weapon weapon)
    {
        if (weapon.weaponDetails.hasInfiniteClipCapacity)
            return;

        StopReloadWeaponCoroutine();
        UpdateReloadText(weapon);

        _reloadWeaponCoroutine = StartCoroutine(UpdateWeaponReloadBarRoutine(weapon));

    }

    private IEnumerator UpdateWeaponReloadBarRoutine(Weapon weapon)
    {
        _barImage.color = Color.red;

        while (weapon.isWeaponReloading)
        {
            float barFill = weapon.weaponReloadTimer / weapon.weaponDetails.weaponReloadTime;

            _reloadBar.transform.localScale = new Vector3(barFill, 1f, 1f);

            yield return null;
        }
    }

    private void ResetWeaponReloadBar()
    {
        StopReloadWeaponCoroutine();

        _barImage.color = Color.green;

        _reloadBar.transform.localScale = new Vector3(1f, 1f, 1f);
    }

    private void StopReloadWeaponCoroutine()
    {
        if (_reloadWeaponCoroutine != null)
        {
            StopCoroutine(_reloadWeaponCoroutine);
        }
    }

    private void UpdateReloadText(Weapon weapon)
    {
        if ((!weapon.weaponDetails.hasInfiniteClipCapacity) && (weapon.weaponClipAmmoRemaining <= 0 || weapon.isWeaponReloading))
        {
            _barImage.color = Color.red;

            StopBlinkingReloadTextCoroutine();

            _blinkingReloadTextCoroutine = StartCoroutine(StartBlinkingReloadTextRoutine());
        }
        else
            StopBlinkingReloadText();
    }

    private IEnumerator StartBlinkingReloadTextRoutine()
    {
        while (true)
        {
            _reloadText.text = "RELOADING";
            yield return new WaitForSeconds(0.3f);
            _reloadText.text = "";
            yield return new WaitForSeconds(0.3f);
        }
    }

    private void StopBlinkingReloadText()
    {
        StopBlinkingReloadTextCoroutine();
        _reloadText.text = "";
    }

    private void StopBlinkingReloadTextCoroutine()
    {
        if (_blinkingReloadTextCoroutine != null)
        {
            StopCoroutine(_blinkingReloadTextCoroutine);
        }
    }
}