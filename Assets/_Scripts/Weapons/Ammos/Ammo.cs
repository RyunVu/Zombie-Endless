using UnityEngine;

public class Ammo : MonoBehaviour, IFireable
{
    private AmmoDetailsSO _ammoDetails;

    #region Tooltip
    [Tooltip("Populate with child TrailRenderer component")]
    #endregion Tooltip
    [SerializeField] private TrailRenderer _ammoTrailRenderer;
    private float _ammoRange = 0f;
    private float _ammoSpeed = 0f;
    private Vector3 _fireDirectionVector;
    private float _fireDirecionAngle;   
    private SpriteRenderer _spriteRenderer;

    // Charging ammo variables
    private float _ammoChargeTimer;                 
    private bool _isAmmoMaterialSet = false;
    private bool _overrideAmmoMovement = false;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Charging ammo before release 
        if (_ammoChargeTimer > 0f)
        {
            _ammoChargeTimer -= Time.deltaTime;
            return;
        }
        // Set the ammo material to the standard ammo material after charging
        else if (_isAmmoMaterialSet == false)
        {
            SetAmmoMaterial(_ammoDetails.ammoMaterial);
            _isAmmoMaterialSet = true;
        }

        // Calculate distance vector to move ammo 
        Vector3 distanceVector = _fireDirectionVector * _ammoSpeed * Time.deltaTime;

        transform.position += distanceVector;

        // Reduce the ammo range
        _ammoRange -= distanceVector.magnitude;
        if (_ammoRange <= 0f)
        {
            DisableAmmo();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        DisableAmmo();
    }

    /// <summary>
    /// Initialise the ammo fire direction vector and angle
    /// </summary>
    public void InitialiseAmmo(AmmoDetailsSO ammoDetails, float aimAngle, float weaponAimAngle, float ammoSpeed, Vector3 weaponAimDirectionVector, bool overrideAmmoMovement = false)
    {
        #region Initialise ammo 
        _ammoDetails = ammoDetails;
        SetFireDirection(_ammoDetails, aimAngle, weaponAimAngle, weaponAimDirectionVector);

        _spriteRenderer.sprite = _ammoDetails.ammoSprite;

        if (_ammoDetails.ammoChargeTime > 0f)
        {
            _ammoChargeTimer = _ammoDetails.ammoChargeTime;
            SetAmmoMaterial(_ammoDetails.ammoChargeMaterial);
            _isAmmoMaterialSet = false;
        }
        else
        {
            _ammoChargeTimer = 0f;
            SetAmmoMaterial(_ammoDetails.ammoMaterial);
            _isAmmoMaterialSet = true;
        }

        _ammoRange = _ammoDetails.ammoRange;
        _ammoSpeed = ammoSpeed;
        _overrideAmmoMovement = overrideAmmoMovement;

        gameObject.SetActive(true);
        #endregion

        #region Initialise ammo trails
        if (_ammoDetails.isAmmoTrail) {
            _ammoTrailRenderer.gameObject.SetActive(true);
            _ammoTrailRenderer.emitting = true;
            _ammoTrailRenderer.material = _ammoDetails.ammoTrailMaterial;
            _ammoTrailRenderer.startWidth = _ammoDetails.ammoTrailStartWidth;
            _ammoTrailRenderer.endWidth = _ammoDetails.ammoTrailEndWidth;
            _ammoTrailRenderer.time = _ammoDetails.ammoTrailTime;
        }
        else {
            _ammoTrailRenderer.emitting = false;
            _ammoTrailRenderer.gameObject.SetActive(false);
        }     
        #endregion
    }

    private void SetFireDirection(AmmoDetailsSO ammoDetails, float aimAngle, float weaponAimAngle, Vector3 weaponAimDirectionVector)
    {
        float randomSpread = Random.Range(ammoDetails.ammoSpreadMin, ammoDetails.ammoSpreadMax);

        // Get a random spread toggle of either -1 or +1
        int spreadToggle = Random.Range(0, 2) * 2 - 1;

        if (weaponAimDirectionVector.magnitude < Settings.useAimAngleDistance)
        {
            // Use the player aim angle
            _fireDirecionAngle = aimAngle;
        }
        else
        {
            // Use the weapon aim angle
            _fireDirecionAngle = weaponAimAngle;
        }

        // Apply random spread
        _fireDirecionAngle += spreadToggle * randomSpread;

        // Set ammo rotation
        transform.eulerAngles = new Vector3(0f, 0f, _fireDirecionAngle);

        // Set ammo fire direction vector
        _fireDirectionVector = HelperUtilities.GetDirectionVectorFromAngle(_fireDirecionAngle);
    }

    private void SetAmmoMaterial(Material material)
    {
        _spriteRenderer.material = material;
    }

    private void DisableAmmo()
    {
        gameObject.SetActive(false);
    }

    public GameObject GetGameObject()
    {
        return this.gameObject;
    }

}