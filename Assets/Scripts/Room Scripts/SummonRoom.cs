using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonRoom : MonoBehaviour
{
    private Player player;
    public GameObject weapon;
    public GameObject summonVFX;
    public GameObject damagedVFX;
    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasTriggered) 
            return;

        player = other.GetComponent<Player>();
        if (player != null)
        {
            float healthSacrifice = player.playerStats.maxHealth * 0.4f;
            player.TakeDamage(healthSacrifice);
            summonVFX.SetActive(true);
            damagedVFX.SetActive(true);
            weapon.SetActive(true);

            hasTriggered = true; 
        }
    }
}

