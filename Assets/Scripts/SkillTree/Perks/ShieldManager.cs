using System;
using UnityEngine;

public class ShieldManager : MonoBehaviour, INeedUI
{
    [SerializeField] PlayerShield playerShield;
    [SerializeField] Player player;
    [SerializeField] float shieldCooldown, cooldownTimer;
    [SerializeField] float regenTimer;
    float regenRatePercent;
    float regenDelay;
    private float yOffSet = 1.9f;

    public event Action OnCoolDown;

    private void Update()
    {
        ShieldBrokenCooldown();

        if (player != null)
        {
            playerShield.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + yOffSet, player.transform.position.z);
        }

        if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.JoystickButton2)) && playerShield.shieldHP > 0)
        {
            playerShield.gameObject.SetActive(true);
            regenTimer = 0;
        }
        else
        {
            playerShield.gameObject.SetActive(false);
            regenTimer += Time.deltaTime;
        }

        if (regenTimer >= regenDelay && playerShield.shieldHP < playerShield.maxShieldHP && cooldownTimer == 0)
        {
            playerShield.shieldHP += playerShield.maxShieldHP * regenRatePercent * Time.deltaTime;
            playerShield.shieldHP = Mathf.Min(playerShield.shieldHP, playerShield.maxShieldHP);
        }
    }


    public void GimmieShield(GameObject shield, float coolDown, float regenDelay, float regenDuration, Player player)
    {
        playerShield = shield.GetComponent<PlayerShield>();
        playerShield.gameObject.SetActive(false);
        shieldCooldown = coolDown;
        regenRatePercent = 100 / (regenDuration * 100);
        this.player = player;
        this.regenDelay = regenDelay;
    }

    public void ShieldBrokenCooldown()
    {
        if (playerShield.shieldHP <= 0)
        {
            if(cooldownTimer == 0)
            {
                OnCoolDown?.Invoke();
            }

            cooldownTimer += Time.deltaTime;

            if (cooldownTimer >= shieldCooldown)
            {
                cooldownTimer = 0;
                playerShield.shieldHP = playerShield.maxShieldHP;
            }
        }
    }

}
