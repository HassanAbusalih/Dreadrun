using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PayloadStats : MonoBehaviour,IDamagable
{
    [Header("Speed Settings")]
    public float onePlayerSpeed, twoPlayerSpeed, threePlayerSpeed, reverseSpeed;

    [Header("Health Settings")]
    [SerializeField] Image healthBar;
    [SerializeField] float maxPayloadHealth, payloadHealth;

    [Header("EXP Settings")]
    public float storedEXP;
    
    void Start()
    {
        payloadHealth = maxPayloadHealth;
    }

    public void TakeDamage(float Damage)
    {
        payloadHealth -= Damage;
        healthBar.fillAmount = payloadHealth / maxPayloadHealth;
        if(payloadHealth <= 0)
        {
            GameManager.Instance.Lose();
        }
    }

    public void AddEXP(float EXP)
    {
        storedEXP += EXP;
    }
}
    