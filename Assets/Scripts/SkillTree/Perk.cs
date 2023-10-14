using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "PlayerPerks")]
public class Perk : ScriptableObject
{
    public string perkName, perkDescription;
    public Sprite perkIcon;

    public virtual void ApplyPlayerBuffs(PlayerStats player)
    {
    }

    public virtual void ApplyPlayerAbilities(PlayerStats player)
    {
    }

    public virtual void ApplyPlayerAoe(PlayerStats player)
    {
    }
}
