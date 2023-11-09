using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlayerPerks/AttackUp")]
public class AttackUp : Perk
{
    [SerializeField]
    private float attackScaling = 20f;
    [SerializeField]
    private int ATKIncrease;

    public override void ApplyPlayerBuffs(Player player)
    {
        if(player.playerWeapon != null)
        {
            player.playerWeapon.DamageModifier /= player.playerStats.attack;
        }

        float increaseAmount = player.playerStats.attack * (attackScaling / 100f);
        player.playerStats.attack += increaseAmount;

        if (player.playerWeapon != null)
        {
            player.playerWeapon.DamageModifier *= player.playerStats.attack;
        }
    }
}
