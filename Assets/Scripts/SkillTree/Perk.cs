using UnityEngine;

public class Perk : ScriptableObject
{
    public string perkName, perkDescription;
    public Sprite perkIcon;

    public virtual void ApplyPlayerBuffs(Player player)
    {
    }

}
