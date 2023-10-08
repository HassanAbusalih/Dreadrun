using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playerstats 
{
    public int health;
    public int maxHealth;
    public int damage;
    public float stamina;
    public float maxStamina;

    public Playerstats(int maxHealth, int damage, float maxStamina)
    {
        this.maxHealth = maxHealth;
        this.health = maxHealth;
        this.damage = damage;
        this.maxStamina = maxStamina;
        this.stamina = maxStamina;
    }

    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;
        if (health <= 0)
        {
            Debug.Log("player die");
        }
    }

    public void ConsumeStamina(int drainAmount)
    {
        stamina -= drainAmount;
        if(stamina <= 0)
        {

        }
    }
    public void GainStamina(int GainAmount)
    {
        stamina -= GainAmount;
        if (stamina > maxStamina)
        {
            stamina = maxStamina;
        }
    }
    public void Heal(int healAmount)
    {
        health += healAmount;
        if (health > maxHealth)
        {
            health = maxHealth;
        }
    }
}
