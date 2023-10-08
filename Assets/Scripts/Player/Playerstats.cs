using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playerstats : MonoBehaviour
{
    [SerializeField] float maxHealth = 100f;
    [SerializeField] float currentHealth;
    [SerializeField] float maxStamina = 100f;
    [SerializeField] float currentStamina;


    void Start()
    {
        currentHealth = maxHealth;
        currentStamina = maxStamina;
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth); 

        if (currentHealth <= 0f)
        {
            Die(); 
        }
    }

    public void GainHealth(float healAmount)
    {
        currentHealth += healAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
    }

    public void ConsumeStamina(float staminaCost)
    {
        currentStamina -= staminaCost;
        currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
    }

    public void RegainStamina(float staminaAmount)
    {
        currentStamina += staminaAmount;
        currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
    }

    void Die()
    {
        Debug.Log("Player die");
    }
}
