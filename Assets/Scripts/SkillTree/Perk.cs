using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "PlayerPerks")]
public class Perk : ScriptableObject
{
    public string perkName, perkDescription;
    public Sprite perkIcon;

    public virtual void ApplyPlayerBuffs(Player player)
    {
    }

    public virtual void ApplyPlayerAbilities(Player player)
    {
    }

    public virtual void ApplyPlayerAoe(Player player)
    {
    }
}
