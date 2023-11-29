using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Potion", menuName = "Inventory/AttackSpeed Potion")]

public class AttackSpeedPotion : ItemBase
{
    [SerializeField] float increaseWeaponDamage;
    float defaultWeaponDamage;

    [SerializeField] float duration;
    float timer = 0f;

    [SerializeField] GameObject timerPrefab;
    Player playerRef;
    public override void UseOnSelf(Player player)
    {
        hasBuffedItem = true;
        if (hasBuffedItem && player.playerWeapon != null)
        {
            defaultWeaponDamage = player.playerWeapon.DamageModifier;
            player.playerWeapon.DamageModifier += increaseWeaponDamage;
            GetTimer();
            playerRef = player;
        }
    }

    void GetTimer()
    {
        Timer timerScript = Instantiate(timerPrefab).GetComponent<Timer>();
        timerScript.SetTimerAndDuration(timer, duration);
        timerScript.onTimerMet += ResetProjectileSpeed;
    }

    void ResetProjectileSpeed()
    {
        playerRef.playerWeapon.DamageModifier = defaultWeaponDamage;
        hasBuffedItem = false;
    }

}

