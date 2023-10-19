using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "New Potion", menuName = "Inventory/Speed Potion")]
public class SpeedPotion : ItemBase
{
    [SerializeField] int speedIncrease;
    public override void UseOnSelf(Player player)
    {
        player.playerStats.speed = speedIncrease;
    }
}
