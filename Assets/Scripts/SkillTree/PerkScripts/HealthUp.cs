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
        float increaseAmount = player.playerStats.health * (HPScaling / 100f);
        player.playerStats.health += increaseAmount;
        player.ScaleWeapon();
        Debug.Log("hp increased");
    }
}
