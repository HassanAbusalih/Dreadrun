using System;
using System.Collections;
using UnityEngine;

public class Berserk : MonoBehaviour, INeedUI
{
    float cooldown;
    float duration;
    float percentDamageIncrease;
    float percentDefenseIncrease;
    float percentSpeedIncrease;
    float percentHealthDrain;
    Player player;
    float healthToDrain;
    float lastDrainTime;
    [SerializeField] float cooldownTimer;
    [SerializeField] bool active = false;
    GameObject berserkEffect;

    public string Keybind { get; private set; } = "Z";

    public event Action OnCoolDown;

    public void Initialize(Player player, float duration, float percentDamageIncrease, float percentDefenseIncrease, float percentSpeedIncrease, float percentHealthDrain, float cooldown, GameObject berserkEffect)
    {
        this.duration = duration;
        this.percentDamageIncrease = percentDamageIncrease;
        this.percentDefenseIncrease = percentDefenseIncrease;
        this.percentSpeedIncrease = percentSpeedIncrease;
        this.percentHealthDrain = percentHealthDrain;
        this.player = player;
        this.cooldown = cooldown;
        this.berserkEffect = Instantiate(berserkEffect, transform.position, Quaternion.identity, transform);
        this.berserkEffect.SetActive(false);
    }

    private void Update()
    {
        if (player == null)
        {
            return;
        }

        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0)
            {
                cooldownTimer = 0;
            }
        }

        if (Input.GetKey(KeyCode.Z) && !active && cooldownTimer == 0)
        {
            ApplyBerserk();
        }
    }

    void ApplyBerserk()
    {
        active = true;
        cooldownTimer = cooldown;
        player.playerStats.attack += player.playerStats.attack * (percentDamageIncrease / 100);
        player.playerStats.defense += player.playerStats.defense * (percentDefenseIncrease / 100);
        player.playerStats.speed += player.playerStats.speed * (percentSpeedIncrease / 100);
        healthToDrain = player.playerStats.maxHealth * (percentHealthDrain / 100);
        berserkEffect.SetActive(true);
        OnCoolDown?.Invoke();
        Invoke(nameof(RemoveBerserk), duration);
        StartCoroutine(DrainHealth());
    }

    void RemoveBerserk()
    {
        active = false;
        player.playerStats.attack -= player.playerStats.attack * (percentDamageIncrease / 100);
        player.playerStats.defense -= player.playerStats.defense * (percentDefenseIncrease / 100);
        player.playerStats.speed -= player.playerStats.speed * (percentSpeedIncrease / 100);
        berserkEffect.SetActive(false);
    }

    IEnumerator DrainHealth()
    {
        while (active)
        {
            if (Time.time - lastDrainTime > 1)
            {
                lastDrainTime = Time.time;
                player.TakeDamage(healthToDrain / duration);
            }
            yield return null;
        }
    }
}