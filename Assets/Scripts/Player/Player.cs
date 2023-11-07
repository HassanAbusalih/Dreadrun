using System;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour, IDamagable
{
    Rigidbody rb;
    Dashing playerDash;
    [Header("Player Input Settings")]
    [SerializeField] KeyCode pickUpWeaponKey = KeyCode.E;
    [SerializeField] KeyCode dropWeaponKey = KeyCode.Q;

    [Header("EquippedWeaponInfo")]
    public PlayerStats playerStats;
    [SerializeField] Slider healthBar;
    [SerializeField] Slider staminaBar;

    [Header("EquippedWeaponInfo")]
    [SerializeField] Transform weaponEquipPosition;
    [SerializeField] WeaponIDs weaponIDsSO;
    int currentWeaponID;
    public PlayerWeapon playerWeapon;
    bool isWeaponPickedUp;

    public Action OnPlayerDeath;
    public delegate bool takeDamage();
    public takeDamage canPLayerTakeDamage;

    [SerializeField] SoundSO takeDamageSFX;
    float timer;

    private void OnEnable()
    {
        playerDash = GetComponent<Dashing>();
        if (playerDash == null) return;
        playerDash.onDashing += ChangeStamina;
        playerDash.canPlayerDash += GetPlayerStamina;
    }

    private void OnDisable()
    {
        if (playerDash == null) return;
        playerDash.onDashing -= ChangeStamina;
        playerDash.canPlayerDash -= GetPlayerStamina;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        weaponIDsSO.InitializeWeaponIDsDictionary();
        playerStats.health = playerStats.maxHealth;
        playerStats.stamina = playerStats.maxStamina;
        InitializePlayerUI(healthBar, playerStats.maxHealth, playerStats.health);
        InitializePlayerUI(staminaBar, playerStats.maxStamina, playerStats.stamina);
    }

    void InitializePlayerUI(Slider playerUiBar, float MaxValue, float CurrentValue)
    {
        if (playerUiBar == null) return;
        playerUiBar.maxValue = MaxValue;
        playerUiBar.value = CurrentValue;
    }

    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        rb.velocity = new Vector3(horizontal * playerStats.speed, 0, vertical * playerStats.speed);
        PickUpUnequippedWeapon();
        DropCurrentWeapon();
    }

    private void PickUpUnequippedWeapon()
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

    public void TakeDamage(float amount)
    {
        if (TryGetComponent(out CounterBlast counterBlast))
        {
            counterBlast.Explode(amount * 0.5f);
        }
        bool _allowToTakeDamage = canPLayerTakeDamage?.Invoke() ?? true;
        if (_allowToTakeDamage) return;
        ChangeHealth(-amount);

    }

    public void ChangeHealth(float amount)
    {
        playerStats.health += amount;
        playerStats.health = Mathf.Clamp(playerStats.health, 0, playerStats.maxHealth);
        UpdatePlayerUI(healthBar, playerStats.health);
        if (playerStats.health <= 0)
        {
            OnPlayerDeath?.Invoke();
        }
    }

    void ChangeStamina(float amount)
    {
        playerStats.stamina += amount;
        playerStats.stamina = Mathf.Clamp(playerStats.stamina, 0, playerStats.maxStamina);
        UpdatePlayerUI(staminaBar, playerStats.stamina);
    }

    void UpdatePlayerUI(Slider playerUiBar, float CurrentValue)
    {
        if (playerUiBar == null) return;
        playerUiBar.value = CurrentValue;
    }

    float GetPlayerStamina()
    {
        return playerStats.stamina;
    }

    void ScaleOrDescaleWeapon(bool _scaled)
    {
        if (playerWeapon == null) return;
        float _scaleFactor = _scaled ? 1 / playerStats.attackSpeed : playerStats.attackSpeed;
        playerWeapon.FireRate *= _scaleFactor;
        _scaleFactor = _scaled ? playerStats.attack : 1 / playerStats.attack;
        playerWeapon.DamageModifier *= _scaleFactor;
        _scaleFactor = _scaled ? playerStats.Range : 1 / playerStats.Range;
        playerWeapon.ProjectileRange *= _scaleFactor;
        _scaleFactor = _scaled ? playerStats.Spread : 1 / playerStats.Spread;
        playerWeapon.SpreadAngle *= _scaleFactor;
    }
}
