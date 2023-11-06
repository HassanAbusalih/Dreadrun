using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour, IDamagable
{
    Rigidbody rb;
    [Header("Player Input Settings")]
    [SerializeField] KeyCode pickUpWeaponKey = KeyCode.E;
    [SerializeField] KeyCode dropWeaponKey = KeyCode.Q;

    // player stats 
    [SerializeField]
    public Slider healthBar;
    public Slider staminaBar;
    public PlayerStats playerStats;

    [Header("EquippedWeaponInfo")]
    [SerializeField] Transform weaponEquipPosition;
    [SerializeField] WeaponIDs weaponIDsSO;
    [SerializeField] int currentWeaponID;
    public PlayerWeapon playerWeapon;
    bool isWeaponPickedUp;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        weaponIDsSO.InitializeWeaponIDsDictionary();
        playerStats.health = playerStats.maxHealth;
        playerStats.stamina = playerStats.maxStamina;

        if (healthBar != null)
        {
            healthBar.maxValue = playerStats.maxHealth;
            healthBar.value = playerStats.health;
        }
        if (staminaBar != null)
        {
            staminaBar.maxValue = playerStats.maxStamina;
            staminaBar.value = playerStats.stamina;
        }
    }

    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        rb.velocity = new Vector3(horizontal * playerStats.speed, 0, vertical * playerStats.speed);
        PickUpUnequippedWeapon();
        DropCurrentWeapon();
    }

    void PickUpUnequippedWeapon()
    {
        if (pickUpWeaponKey == KeyCode.None) pickUpWeaponKey = KeyCode.E;
        if (playerWeapon == null) return;
        if (Input.GetKeyDown(pickUpWeaponKey) && !isWeaponPickedUp)
        {
            ScaleOrDescaleWeapon(true);
            playerWeapon.PickUpWeapon(weaponEquipPosition, ref currentWeaponID);
            isWeaponPickedUp = true;
        }
    }

    void DropCurrentWeapon()
    {
        if (dropWeaponKey == KeyCode.None) dropWeaponKey = KeyCode.Q;
        if (Input.GetKeyDown(dropWeaponKey) && isWeaponPickedUp)
        {
            ScaleOrDescaleWeapon(false);
            playerWeapon.DropWeapon();
            isWeaponPickedUp = false;
            currentWeaponID = 0;
            playerWeapon = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out PlayerWeapon unequippedWeapon) || isWeaponPickedUp) return;
        playerWeapon = unequippedWeapon;
    }

    private void OnTriggerExit(Collider other)
    {
        if (isWeaponPickedUp) return;
        playerWeapon = null;
    }

    void ChangeHealth(float amount)
    {
        // if (isInvincible) return;
        playerStats.health -= amount;
        playerStats.health = Mathf.Clamp(playerStats.health, 0, playerStats.maxHealth);
        UpdateHealthBar();
        if (playerStats.health <= 0)
        {
            try
            {
                PlayerDeath();
            }
            catch { }
        }
    }

    public void TakeDamage(float amount)
    {
        ChangeHealth(amount);
        UpdateHealthBar();
        if (TryGetComponent(out CounterBlast counterBlast))
        {
            counterBlast.Explode(amount * 0.5f);
        }
    }

    public void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.value = playerStats.health;
        }
    }

    void PlayerDeath()
    {
        GameManager.Instance.Lose();
    }

    public void UpdateStaminaBar()
    {
        if (staminaBar != null)
        {
            staminaBar.value = playerStats.stamina;
        }
    }

    void ScaleOrDescaleWeapon(bool _scaled)
    {
        if (playerWeapon == null) return;
        float _scaleFactor = _scaled ? 1/playerStats.attackSpeed : playerStats.attackSpeed;
        playerWeapon.FireRate *= _scaleFactor;
        _scaleFactor = _scaled ? playerStats.attack : 1/playerStats.attack;
        playerWeapon.DamageModifier *= _scaleFactor;
        _scaleFactor = _scaled ? playerStats.Range : 1/playerStats.Range;
        playerWeapon.ProjectileRange *= _scaleFactor;
        _scaleFactor = _scaled ? playerStats.Spread : 1/playerStats.Spread;
        playerWeapon.SpreadAngle *= _scaleFactor;
    }
}
