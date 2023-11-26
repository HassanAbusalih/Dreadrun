using ClientLibrary;
using NetworkingLibrary;
using System;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour, IDamagable
{
    Rigidbody rb;
    Dashing playerDash;
    [SerializeField] NetworkComponent networkComponent;
    [Header("Player Input/Movement Settings")]
    [SerializeField] KeyCode pickUpWeaponKey = KeyCode.E;
    [SerializeField] KeyCode dropWeaponKey = KeyCode.Q;
    [SerializeField] KeyCode controllerPickUp;
    [SerializeField] KeyCode controllerDrop;
    [SerializeField] float maxSlopeAngle;
    [SerializeField] float UpSlopeGravity;
    [SerializeField] bool onSlope;
    public PlayerStats playerStats;

    [Header("Player UI Settings")]
    [SerializeField] Slider healthBar;
    [SerializeField] Slider staminaBar;

    [Header("EquippedWeaponInfo")]
    [SerializeField] Transform weaponEquipPosition;
    [SerializeField] WeaponIDs weaponIDsSO;
    int currentWeaponID;
    public PlayerWeapon playerWeapon;
    bool isWeaponPickedUp;

    public static Action<GameObject> onDamageTaken;
    public Action OnPlayerDeath;
    public delegate bool takeDamage();
    public takeDamage canPLayerTakeDamage;
   
    [SerializeField] SoundSO takeDamageSFX;

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
        onSlope = isPlayerOnSlope();
        Vector3 moveDirection = GetMovementDirection();
        rb.velocity = new Vector3(moveDirection.x, rb.velocity.y, moveDirection.z);
        SendPositionPacket(transform.position);
        if(onSlope && rb.velocity.y<0 || rb.velocity.y>0) rb.velocity += Vector3.down * UpSlopeGravity;
        PickUpUnequippedWeapon();
        DropCurrentWeapon();
    }


    void  SendPositionPacket(Vector3 PlayerPosition)
    {
        PositionPacket positionPacket = new PositionPacket(PlayerPosition,networkComponent.GameObjectId);
        Client.Instance.SendPacket(positionPacket.Serialize());
    }

    Vector3 GetMovementDirection()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 moveDirection = new Vector3(horizontal, 0, vertical);
        if (isPlayerOnSlope())
        {
            Vector3 slopeDirection = GetSlopeDirection(moveDirection);
            moveDirection = slopeDirection + moveDirection.normalized * playerStats.speed;
        }
        moveDirection = moveDirection.normalized * playerStats.speed;
        return moveDirection;
    }

    bool isPlayerOnSlope()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out slopeHitt, 30f))
        {
            float angle = Vector3.Angle(slopeHitt.normal, Vector3.up);
            transform.rotation = Quaternion.FromToRotation(transform.up, slopeHitt.normal) * transform.rotation;
            return angle > maxSlopeAngle && angle!=0;
        }
        return false;
    }

    Vector3 GetSlopeDirection(Vector3 _moveDirection)
    {
        return Vector3.ProjectOnPlane(_moveDirection, slopeHitt.normal);
    }

    private void PickUpUnequippedWeapon()
    {
        if (pickUpWeaponKey == KeyCode.None) pickUpWeaponKey = KeyCode.E;
        if (controllerPickUp == KeyCode.None) controllerPickUp = KeyCode.JoystickButton3;
        if (playerWeapon == null) return;
        if (Input.GetKeyDown(pickUpWeaponKey) || Input.GetKeyDown(controllerPickUp) && !isWeaponPickedUp)
        {
            ScaleOrDescaleWeapon(true);
            playerWeapon.PickUpWeapon(weaponEquipPosition, ref currentWeaponID);
            isWeaponPickedUp = true;
        }
    }

    void DropCurrentWeapon()
    {
        if(dropWeaponKey == KeyCode.None) dropWeaponKey = KeyCode.Q;
        if (controllerDrop == KeyCode.None) controllerDrop = KeyCode.JoystickButton2;
        if (Input.GetKeyDown(dropWeaponKey)|| Input.GetKeyDown(controllerDrop) && isWeaponPickedUp)
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
        IDamagable.onDamageTaken?.Invoke(gameObject);
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
