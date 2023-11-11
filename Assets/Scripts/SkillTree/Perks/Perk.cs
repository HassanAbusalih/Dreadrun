using JetBrains.Annotations;
using UnityEngine;

public class Perk : Collectable
{
    public bool unique;
    public bool needsUI;
    public virtual void ApplyPlayerBuffs(Player player)
    {
     
    }
    public virtual void ApplyPlayerBuffs(Player player, ref INeedUI needUI)
    {
       
    }

    public virtual float FetchCooldown()
    {
        return float.MinValue;
    }

}
