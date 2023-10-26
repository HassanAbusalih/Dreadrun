using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldManager : MonoBehaviour
{
    [SerializeField] private PlayerShield playerShield;
    [SerializeField] private Player player;
    [SerializeField] private float shieldCooldown, cooldownTimer;
    

    private void Update()
    {
        ShieldBrokenCooldown();

        if (player != null)
        {
            playerShield.transform.position = player.transform.position;
        }

        if (Input.GetKey(KeyCode.LeftShift) && playerShield.shieldHP > 0)
        {
            playerShield.gameObject.SetActive(true);
        }
        else
        {
            playerShield.gameObject.SetActive(false);
        }
    }
      public void GimmieShield(GameObject shield, float coolDown, Player player)
    {
        playerShield = shield.GetComponent<PlayerShield>();
        this.player = player;
        shieldCooldown = coolDown;
    }

    public void ShieldBrokenCooldown()
    {
        if (playerShield.shieldHP <= 0)
        {
            cooldownTimer += Time.deltaTime;

            if (cooldownTimer >= shieldCooldown)
            {
                cooldownTimer = 0;
                playerShield.shieldHP = playerShield.maxShieldHP;
            }
        }
    }
}
