using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlayerPerks/AttackSpeed")]
public class AttackSpeed : Perk
{
    [SerializeField]
    private float attackSpeedScaling = 20f;
    [SerializeField]
    private int AddAttackSpeed;

    public override void ApplyPlayerBuffs(Player player)
    {
        float increaseAmount = player.playerStats.attackSpeed * (attackSpeedScaling / 100f);
        player.playerStats.attackSpeed += increaseAmount;
        player.ScaleWeapon();
    }
}
