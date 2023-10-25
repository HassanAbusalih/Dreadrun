using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PayloadStats : MonoBehaviour,IDamagable
{
    [Header("Speed Settings")]
    public float onePlayerSpeed;
    public float twoPlayerSpeed;
    public float threePlayerSpeed;
    public float reverseSpeed;

    [Header("Health Settings")]
    [SerializeField] Image healthBar;
    [SerializeField] float maxPayloadHealth;
    [SerializeField] float payloadHealth;

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
    }

    public void AddEXP(float EXP)
    {
        storedEXP += EXP;
    }
}
    