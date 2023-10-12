using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    //player controls 
    [SerializeField] float speed = 10f;
    [SerializeField] Rigidbody rb;
    [SerializeField] KeyCode dodge;
    [SerializeField] float dashDistance;
    [SerializeField] float dashDuration;
    [SerializeField] bool isDashing;


    // player stats 
    [SerializeField]
    public int health;
    public int maxHealth;
    public int attack;
    public float stamina;
    public float maxStamina;
    public int attackSpeed;
    public Slider healthBar;
    public Slider staminaBar;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        health = maxHealth;
        stamina = maxStamina;
        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = health;
        }
        if(staminaBar != null)
        {
            staminaBar.maxValue = maxStamina;
            staminaBar.value = stamina;
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
            float t = elapsedTime / dashDuration;
            rb.MovePosition(Vector3.Lerp(transform.position, endPosition, t));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        StopDash();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (isDashing)
        {
            if (collision.gameObject.CompareTag("Wall"))
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
    public void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.value = health;
        }
    }

    public void UpdateStaminaBar()
    {
        if (staminaBar != null)
        {
            staminaBar.value = stamina;
        }
    }
}
