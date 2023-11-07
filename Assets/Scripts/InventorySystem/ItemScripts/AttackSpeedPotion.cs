using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Potion", menuName = "Inventory/AttackSpeed Potion")]

public class AttackSpeedPotion : ItemBase
{
    [SerializeField] float increaseProjectileSpeed;
    float defaultProjectileSpeed;

    [SerializeField] float duration;
    float timer = 0f;

    [SerializeField] GameObject timerPrefab;
    Player playerRef;

    [SerializeField] SoundSO attackSpeedSFX;

    public override void UseOnSelf(Player player)
    {
        hasBuffedItem = true;
        defaultProjectileSpeed = player.playerWeapon.ProjectileSpeed;
        player.playerWeapon.ProjectileSpeed += increaseProjectileSpeed;
        attackSpeedSFX.Play();
        Debug.Log("Increased Projectile speed");
        GetTimer();
        playerRef = player;
    }

    void GetTimer()
    {
        Timer timerScript = Instantiate(timerPrefab).GetComponent<Timer>();
        timerScript.SetTimerAndDuration(timer, duration);
        timerScript.onTimerMet += ResetProjectileSpeed;
    }

    void ResetProjectileSpeed()
    {
        playerRef.playerWeapon.ProjectileSpeed = defaultProjectileSpeed;
        hasBuffedItem = false;
    }
}

