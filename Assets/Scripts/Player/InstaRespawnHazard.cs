using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstaRespawnHazard : MonoBehaviour
{
    public float damageAmount;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerRespawn playerRespawn = other.GetComponent<PlayerRespawn>();
            if (playerRespawn != null)
            {
                playerRespawn.Respawn();
            }
        }
    }
}
