using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlayerPerks/AttackSpeed")]
public class AttackSpeed : Perk
{
    [SerializeField]
    private int AddAttackSpeed;

    public override void ApplyPlayerBuffs(Player player)
    {
     
    }
}

