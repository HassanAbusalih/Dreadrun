using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlayerPerks/CounterBlast")]
public class CounterBlastPerk : Perk
{
    public override void ApplyPlayerBuffs(Player player)
    {
        player.gameObject.AddComponent<CounterBlast>();
    }
}
