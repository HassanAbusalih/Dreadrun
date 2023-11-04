using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;

public class PayloadUI : MonoBehaviour
{
    [Header("Health Bar Settings")]
    [SerializeField] Image healthBar;

    [Header("Payload State Settings")]
    [SerializeField] Image payloadStateImage;
    [SerializeField] Sprite[] payloadStatesSprites;

    [Header("Range Indicator Settings")]
    [SerializeField] MeshRenderer rangeIndicatorRingRenderer;
    [SerializeField] MeshRenderer rangeIndicatorFillRenderer;

    [SerializeField] Material[] rangeIndicatorFillMaterials;
    [SerializeField] Material[] rangeIndicatorRingMaterials;

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
        LookAtCamera();
    }

    public void ChangePayloadStateDisplay(int state)
    {
        if (payloadStateImage.sprite != payloadStatesSprites[state])
        {
            payloadStateImage.sprite = payloadStatesSprites[state];

            switch (state)
            {
                case 0:
                    rangeIndicatorFillRenderer.material = rangeIndicatorFillMaterials[0];
                    rangeIndicatorRingRenderer.material = rangeIndicatorRingMaterials[0];
                    break;

                case 1:
                case 2:
                case 3:
                    rangeIndicatorFillRenderer.material = rangeIndicatorFillMaterials[1];
                    rangeIndicatorRingRenderer.material = rangeIndicatorRingMaterials[1];
                    break;

                case 4:
                    rangeIndicatorFillRenderer.material = rangeIndicatorFillMaterials[2];
                    rangeIndicatorRingRenderer.material = rangeIndicatorRingMaterials[2];
                    break;
            }
        }

    }

    public void LookAtCamera()
    {
        transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);

    }
}