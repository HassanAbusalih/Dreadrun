using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour, IDamagable
{
    //player controls 
    [SerializeField] float speed = 10f;
    [SerializeField] Rigidbody rb;
    [SerializeField] KeyCode dodge;
    [SerializeField] float dashDistance;
    [SerializeField] float dashDuration;
    [SerializeField] bool isDashing;
    [SerializeField] LayerMask ground;

    // player stats 
    [SerializeField]
    public Slider healthBar;
    public Slider staminaBar;
    public PlayerStats playerStats;

    private void Start()
    {
        //playerStats = new PlayerStats();
        //eventually make it so it sets the player stats to a serialized list of per-player stats.
        
        rb = GetComponent<Rigidbody>();
        playerStats.health = playerStats.maxHealth;
        playerStats.stamina = playerStats.maxStamina;
        if (healthBar != null)
        {
            healthBar.maxValue = playerStats.maxHealth;
            healthBar.value = playerStats.health;
        }
        if(staminaBar != null)
        {
            staminaBar.maxValue = playerStats.maxStamina;
            staminaBar.value = playerStats.stamina;
        }
    }

    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        rb.velocity = new Vector3(horizontal * speed, 0, vertical * speed);
        if (Input.GetKeyDown(KeyCode.Space) && !isDashing)
        {
            Vector3 dashDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")).normalized;
            StartCoroutine(Dash(dashDirection));
        }
    }

    IEnumerator Dash(Vector3 dashDirection)
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
    private void StopDash()
    {
        isDashing = false;
        rb.useGravity = true;
        rb.freezeRotation = false;
    }

    public void TakeDamage(float amount)
    {
        playerStats.health -= amount;
        UpdateHealthBar();
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
}
