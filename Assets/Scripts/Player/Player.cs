using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour, IDamagable
{
    // player controls 
    [SerializeField] Rigidbody rb;
    [SerializeField] KeyCode dodge;
    [SerializeField] float dashDistance;
    [SerializeField] float dashDuration;
    [SerializeField] bool isDashing;
    [SerializeField] LayerMask ground;
    [SerializeField] GameManager manager;
    // player stats 
    [SerializeField]
    public Slider healthBar;
    public Slider staminaBar;
    public PlayerStats playerStats;

    // player weapon, etc
    [SerializeField]
    public PlayerWeapon playerWeapon;
    [SerializeField] bool isWeaponPickedUp;
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
        rb.velocity = new Vector3(horizontal * playerStats.speed, rb.velocity.y, vertical * playerStats.speed);
    }




    private void OnTriggerEnter(Collider other)
    {
        PickUpWeaponOnTrigger(other.gameObject);
    }

    void PickUpWeaponOnTrigger(GameObject _weaponCollided)
    {
        if (isWeaponPickedUp) return;

        if (_weaponCollided.TryGetComponent(out PlayerWeapon weaponToEquip))
        {
            weaponToEquip.PickUpWeapon(weaponEquipPosition, ref currentWeaponID);
            playerWeapon = weaponToEquip;
            isWeaponPickedUp = true;
        }
    }

 

    public void TakeDamage(float amount)
    {
        if (TryGetComponent(out CounterBlast counterBlast))
        {
            counterBlast.Explode(amount * 0.5f);
        }

        playerStats.health -= amount;
        UpdateHealthBar();
        if (playerStats.health <= 0)
        {
            //manager.LoseState();
        }
    }

    public void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.value = playerStats.health;
        }
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
        if (playerWeapon == null)
        {
            return;
        }
        playerWeapon.FireRate *= playerStats.attackSpeed;
        playerWeapon.DamageModifier *= playerStats.attack;
        playerWeapon.ProjectileRange *= playerStats.Range;
    }

}
