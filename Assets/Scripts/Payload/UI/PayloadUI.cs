using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PayloadUI : MonoBehaviour
{
    [SerializeField] Image healthBar;
    [SerializeField] Image payloadStateImage;
    [SerializeField] Sprite[] payloadStateSprites;
    PayloadStats payloadStats;

    void Start()
    {
        payloadStats = FindObjectOfType<PayloadStats>();
    }

    void Update()
    {
        healthBar.fillAmount = payloadStats.payloadHealth / payloadStats.maxPayloadHealth;
    }


    void LateUpdate()
    {
        transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
    }

    public void ChangePayloadUISprite(int state)
    {
        if(payloadStateImage.sprite != payloadStateSprites[state])
            payloadStateImage.sprite = payloadStateSprites[state];
    }
}