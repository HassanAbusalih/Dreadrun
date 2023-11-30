using System;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class Player : MonoBehaviour, IDamagable
{
    Rigidbody rb;
    Dashing playerDash;
    CinemachineImpulseSource impulseSource;
    [Header("Player Input Settings")]
    [SerializeField] KeyCode pickUpWeaponKey = KeyCode.E;
    [SerializeField] KeyCode controllerPickUp;
    [SerializeField] float durationToDropWeapon = 1.5f;


    [Header("Player slope info/Settings")]
    [SerializeField] float maxSlopeAngle;
    [SerializeField] float UpSlopeGravity;
    [SerializeField] bool onSlope;

    [Header("Player Stats & Movement Settings")]
    [SerializeField] float slowDownMultiplier;
    [SerializeField] AnimationCurve slowDownCurve;
    [SerializeField][Range(0, 2)] float staminaRecoverySpeed;
    public PlayerStats playerStats;

    [Header("Player UI Settings")]
    [SerializeField] Image healthBar;
    [SerializeField] Image staminaBar;

    [Header("EquippedWeaponInfo")]
    [SerializeField] Transform weaponEquipPosition;
    [SerializeField] WeaponIDs weaponIDsSO;

    int currentWeaponID;
    public PlayerWeapon playerWeapon;
    bool isWeaponPickedUp;
    bool isPickUpMode;

    // Player Events
    public static Action<GameObject> onDamageTaken;
    public Action<float,bool> WeaponDropFeedback;
    public Action OnPlayerDeath;
    public delegate bool takeDamage();
    public takeDamage canPLayerTakeDamage;

    [SerializeField] SoundSO takeDamageSFX;
    [SerializeField] float sfxCooldown = 1f;
    float timeSinceSFX;
    float timer;
    RaycastHit slopeHitt;

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
        timeSinceSFX = Time.time;
        rb = GetComponent<Rigidbody>();
        weaponIDsSO.InitializeWeaponIDsDictionary();
        playerStats.health = playerStats.maxHealth;
        playerStats.stamina = playerStats.maxStamina;
        InitializePlayerUI(healthBar, playerStats.maxHealth, playerStats.health);
        InitializePlayerUI(staminaBar, playerStats.maxStamina, playerStats.stamina);
        TryGetComponent(out impulseSource);
    }

    void InitializePlayerUI(Image playerUiBar, float MaxValue, float CurrentValue)
    {
        if (playerUiBar == null) return;
        playerUiBar.fillAmount = CurrentValue / MaxValue;
    }

    private void Update()
    {
        DropCurrentWeapon();
        PickUpUnequippedWeapon();
        
        playerStats.stamina = Mathf.Lerp(playerStats.stamina, playerStats.maxStamina, Time.deltaTime * staminaRecoverySpeed);
        UpdatePlayerUI(staminaBar, playerStats.stamina, playerStats.maxStamina);
    }

    private void LateUpdate()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 normalizedDirection = new Vector3(horizontal, 0, vertical).normalized;
        Vector3 slopeDirection = isPlayerOnSlope(normalizedDirection).Item1;
        onSlope = isPlayerOnSlope(normalizedDirection).Item2;
        Vector3 moveVelocity = (slopeDirection + normalizedDirection) * playerStats.speed;

        if( normalizedDirection.magnitude!=0)
        rb.AddForce(Vector3.down * UpSlopeGravity, ForceMode.Acceleration);
        if( normalizedDirection.magnitude == 0 && !onSlope && rb.velocity.y<=0)
        rb.AddForce(Vector3.down * UpSlopeGravity, ForceMode.Acceleration);
        rb.useGravity = !onSlope;

        rb.velocity += moveVelocity;
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, playerStats.speed);

        if (horizontal != 0 && vertical != 0) return;
        float slowDownLerpValue = slowDownCurve.Evaluate(Time.fixedDeltaTime * slowDownMultiplier);
        rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, slowDownLerpValue);

    }

    (Vector3, bool) isPlayerOnSlope(Vector3 _moveDirection)
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHitt, 30f))
        {
            float angle = Vector3.Angle(slopeHitt.normal, Vector3.up);
            transform.rotation = Quaternion.FromToRotation(transform.up, slopeHitt.normal) * transform.rotation;
            bool isOnSlope = angle > maxSlopeAngle && angle != 0;
            Vector3 slopeDirection = Vector3.ProjectOnPlane(_moveDirection, slopeHitt.normal);
            return (slopeDirection, isOnSlope);
        }
        return (Vector3.zero, false);
    }


    private void PickUpUnequippedWeapon()
    {
        if (pickUpWeaponKey == KeyCode.None) pickUpWeaponKey = KeyCode.E;
        if (controllerPickUp == KeyCode.None) controllerPickUp = KeyCode.JoystickButton3;
        
        if (Input.GetKeyDown(pickUpWeaponKey) || Input.GetKeyDown(controllerPickUp))
        {
            if (playerWeapon == null || isWeaponPickedUp) return;
            ScaleOrDescaleWeapon(true);
            playerWeapon.PickUpWeapon(weaponEquipPosition, ref currentWeaponID);
            isWeaponPickedUp = true;
            isPickUpMode = true;
        }
        else if(Input.GetKeyUp(pickUpWeaponKey) || Input.GetKeyUp(controllerPickUp) && isWeaponPickedUp)
        {
           isPickUpMode = false;
        }
    }

    void DropCurrentWeapon()
    {
        if (Input.GetKey(pickUpWeaponKey) || Input.GetKey(controllerPickUp) && isWeaponPickedUp)
        {
            if(!isWeaponPickedUp || isPickUpMode) return;
            if (timer < durationToDropWeapon)
            { timer += Time.deltaTime; WeaponDropFeedback?.Invoke(durationToDropWeapon, true); return; }
           
            ScaleOrDescaleWeapon(false);
            playerWeapon.DropWeapon();
            isWeaponPickedUp = false;
            currentWeaponID = 0;
            playerWeapon = null;
            timer = 0;
        }
        else {timer = 0; WeaponDropFeedback?.Invoke(durationToDropWeapon,false);}
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out PlayerWeapon unequippedWeapon) || isWeaponPickedUp) return;
        playerWeapon = unequippedWeapon;
    }

    private void OnTriggerExit(Collider other)
    {
        if (isWeaponPickedUp) return;
        if (other.TryGetComponent(out PlayerWeapon unequippedWeapon))
        {
            if(playerWeapon == unequippedWeapon) playerWeapon = null;
        }
    }

    public void TakeDamage(float amount)
    {
        amount = (100 / (100 + playerStats.defense)) * amount;
        if (TryGetComponent(out CounterBlast counterBlast))
        {
            counterBlast.Explode(amount * 0.5f);
        }
        bool _allowToTakeDamage = canPLayerTakeDamage?.Invoke() ?? true;
        if (_allowToTakeDamage) return;

        if (Time.time - timeSinceSFX > sfxCooldown && takeDamageSFX != null)
        {
            takeDamageSFX.PlaySound(0, AudioSourceType.Player);
            timeSinceSFX = Time.time;
        }
        ChangeHealth(-amount);
        IDamagable.onDamageTaken?.Invoke(gameObject);
        if(impulseSource!=null)impulseSource.GenerateImpulse();
    }

    public void ChangeHealth(float amount)
    {
        playerStats.health += amount;
        playerStats.health = Mathf.Clamp(playerStats.health, 0, playerStats.maxHealth);
        UpdatePlayerUI(healthBar, playerStats.health, playerStats.maxHealth);
        if (playerStats.health <= 0)
        {
            OnPlayerDeath?.Invoke();
        }
    }

    void ChangeStamina(float amount)
    {
        playerStats.stamina += amount;
        UpdatePlayerUI(staminaBar, playerStats.stamina, playerStats.maxStamina);
    }

    void UpdatePlayerUI(Image playerUiBar, float CurrentValue, float maxValue)
    {
        if (playerUiBar == null) return;
        playerUiBar.fillAmount = CurrentValue / maxValue;
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
