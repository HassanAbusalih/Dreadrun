using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour, IDamagable
{

    [SerializeField] Rigidbody rb;

    [SerializeField] float dashDistance;
    [SerializeField] float dashDuration;
    [SerializeField] bool isDashing;
    public bool controllerEnabled;
    [SerializeField] LayerMask ground;


    [SerializeField] KeyCode keyDodge;
    [SerializeField] KeyCode pickUpWeaponKey = KeyCode.E;
    [SerializeField] KeyCode dropWeaponKey = KeyCode.Q;
    [SerializeField] KeyCode controllerPickUp;
    [SerializeField] KeyCode controllerDrop;
    // player stats 
    [SerializeField]
    public Slider healthBar;
    public Slider staminaBar;
    public PlayerStats playerStats;

    // player weapon, etc
    [SerializeField]
    public PlayerWeapon playerWeapon;
    bool isWeaponPickedUp;
    [SerializeField] Transform weaponEquipPosition;
    [SerializeField] WeaponIDs weaponIDsSO;
    [SerializeField] int currentWeaponID;

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
        DashOnInput();
        DropCurrentWeapon();
        PickUpUnequippedWeapon();
    }

    private void DashOnInput()
    {
        if (Input.GetKeyDown(keyDodge)||Input.GetKeyDown(KeyCode.JoystickButton0) && !isDashing)
        {
            Vector3 dashDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")).normalized;
            StartCoroutine(StartDash(dashDirection));
        }
    }

    IEnumerator StartDash(Vector3 dashDirection)
    {
        isDashing = true;
        rb.useGravity = false;
        rb.freezeRotation = true;
        Vector3 endPosition = transform.position + dashDirection * dashDistance;
        float elapsedTime = 0f;

        while (elapsedTime < dashDuration && isDashing)
        {
            Vector3 newPosition = Vector3.MoveTowards(transform.position, endPosition, Time.deltaTime * (dashDistance / dashDuration));
            rb.MovePosition(newPosition);
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        StopDash();
    }

    private void StopDash()
    {
        isDashing = false;
        rb.useGravity = true;
        rb.freezeRotation = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isDashing)
        {
            if (collision.gameObject.layer != ground)
            {
                StopDash();
            }
        }
    }

    void PickUpUnequippedWeapon()
    {
        if(pickUpWeaponKey == KeyCode.None) pickUpWeaponKey = KeyCode.E;
        if (controllerPickUp == KeyCode.None) controllerPickUp = KeyCode.JoystickButton3;
        if (playerWeapon == null) return;
        if (Input.GetKeyDown(pickUpWeaponKey) ||Input.GetKeyDown(controllerPickUp) && !isWeaponPickedUp)
        {
            playerWeapon.PickUpWeapon(weaponEquipPosition, ref currentWeaponID);
            isWeaponPickedUp = true;
            ScaleWeapon();
        }
    }

    void DropCurrentWeapon()
    {
        if(dropWeaponKey == KeyCode.None) dropWeaponKey = KeyCode.Q;
        if (controllerDrop == KeyCode.None) controllerDrop = KeyCode.JoystickButton3;
        if (Input.GetKeyDown(dropWeaponKey)|| Input.GetKeyDown(controllerDrop) && isWeaponPickedUp)
        {
            playerWeapon.DropWeapon();
            isWeaponPickedUp = false;
            currentWeaponID = 0;
            playerWeapon = null;
            DeScaleWeapon();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out PlayerWeapon unequippedWeapon) || isWeaponPickedUp) return;
        playerWeapon = unequippedWeapon;
    }

    private void OnTriggerExit(Collider other)
    {
        if(isWeaponPickedUp) return;
        playerWeapon = null;
    }

    public void TakeDamage(float amount)
    {
        playerStats.health -= amount;
        playerStats.health = Mathf.Clamp(playerStats.health, 0, playerStats.maxHealth);
        UpdateHealthBar();
        if (TryGetComponent(out CounterBlast counterBlast))
        {
            counterBlast.Explode(amount * 0.5f);
        }

        if (playerStats.health <= 0)
        {
            try
            {
                PlayerDeath();
            }
            catch { }
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

    public void ScaleWeapon()
    {
        if (playerWeapon == null) return;
        playerWeapon.FireRate *= playerStats.attackSpeed;
        playerWeapon.DamageModifier *= playerStats.attack;
        playerWeapon.ProjectileRange *= playerStats.Range;
    }

    public void DeScaleWeapon()
    {
        if (playerWeapon == null) return;
        playerWeapon.FireRate /= playerStats.attackSpeed;
        playerWeapon.DamageModifier /= playerStats.attack;
        playerWeapon.ProjectileRange /= playerStats.Range;
    }

}
