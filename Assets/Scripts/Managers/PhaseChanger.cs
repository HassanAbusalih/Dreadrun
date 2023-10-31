using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseChanger : MonoBehaviour
{
    private bool playerInside = false;
    private float insideTimer = 0f;
    [SerializeField] float requiredTime = 10f;
    [SerializeField] PayloadMovement payload;
    [SerializeField] GameObject wall;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            playerInside = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Player player) && playerInside)
        {
            playerInside = false;
            insideTimer = 0f;
        }
    }

    private void Update()
    {
        if (playerInside)
        {
            insideTimer += Time.deltaTime;

            if (insideTimer >= requiredTime)
            {
                ChangePhase();
            }
        }
    }

    public void ChangePhase()
    {
        wall.SetActive(false);
        payload.EnableMovement();
    }
}   
