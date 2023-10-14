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
        float increaseAmount = player.playerStats.attack * (attackScaling / 100f);
        player.playerStats.attack += increaseAmount;
        player.ScaleWeapon();
        Debug.Log(player.playerStats.attack);
    }
}
