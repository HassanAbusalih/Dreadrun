using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlayerPerks/HealthUp")]
public class HealthUp : Perk
{
    [SerializeField]
    private float HPScaling = 20f;
    [SerializeField]
    private int AddHP;

    public override void ApplyPlayerBuffs(Player player)
    {
        float increaseAmount = player.playerStats.maxHealth * (HPScaling / 100f);
        player.playerStats.maxHealth += increaseAmount;
        player.playerStats.health += increaseAmount;   
    }
}
