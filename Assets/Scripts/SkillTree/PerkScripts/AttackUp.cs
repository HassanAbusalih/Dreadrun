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

    public override void ApplyPlayerBuffs(PlayerStats player)
    {
        float increaseAmount = player.attack * (attackScaling / 100f); //increase by %
        int roundedATK = Mathf.RoundToInt(increaseAmount); //round it to an int to be able 2 use
        player.attack += roundedATK;
        Debug.Log(player.attack);
    }
}
