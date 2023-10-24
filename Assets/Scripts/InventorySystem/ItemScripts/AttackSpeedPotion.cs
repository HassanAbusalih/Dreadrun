using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Potion", menuName = "Inventory/AttackSpeed Potion")]

public class AttackSpeedPotion : ItemBase
{
    [SerializeField] float speedIncrease;
    public override void UseOnSelf(Player player)
    {
        player.playerWeapon.ProjectileSpeed = speedIncrease;
    }
}

