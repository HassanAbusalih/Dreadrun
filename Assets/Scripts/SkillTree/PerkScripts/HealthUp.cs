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

    public override void ApplyPlayerBuffs(PlayerStats player)
    {
        float increaseAmount = player.health * (HPScaling / 100f); //increase by %
        int roundedHP = Mathf.RoundToInt(increaseAmount); //round it to an int to be able 2 use
        player.health += roundedHP;
    }
}
