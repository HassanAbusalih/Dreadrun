using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PayloadStats : MonoBehaviour
{
    public Image healthBar;

    public float maxPayloadHealth;
    private float payloadHealth;
    
    void Start()
    {
        payloadHealth = maxPayloadHealth;
    }

    
    void Update()
    {
        // for testing, delete later
        if(Input.GetKeyDown(KeyCode.F))
        {
            takeDamage(10);
        }
    }

    public void takeDamage(float Damage)
    {
        payloadHealth -= Damage;
        healthBar.fillAmount = payloadHealth / maxPayloadHealth;
    }
}
    