using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "New Potion", menuName = "Inventory/Healing Potion")]
public class HealingPotion : ItemBase
{
    [SerializeField]
    private int healingAmount;
    
    public override void UseOnSelf(Player player)
    {
        
    }
}
