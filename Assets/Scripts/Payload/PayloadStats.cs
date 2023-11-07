using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PayloadStats : MonoBehaviour,IDamagable
{
    public static PayloadStats instance;

    [Header("Speed Settings")]
    public float onePlayerSpeed;
    public float twoPlayerSpeed;
    public float threePlayerSpeed;
    public float reverseSpeed;

    [Header("Health Settings")]
    public float maxPayloadHealth;
    public float payloadHealth;

    [Header("Range Settings")]
    public float payloadRange;

    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        payloadHealth = maxPayloadHealth;
    }

    public void TakeDamage(float Damage)
    {
        payloadHealth -= Damage;

        if(payloadHealth <= 0)
        {
            GameManager.Instance.Lose();
        }
    }
}
    