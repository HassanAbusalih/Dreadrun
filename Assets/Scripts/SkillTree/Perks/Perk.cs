using JetBrains.Annotations;
using UnityEngine;

public class Perk : Collectable
{
    public bool unique;
    public virtual void ApplyPlayerBuffs(Player player)
    {
     
    } 

    public virtual float FetchCooldown()
    {
        return float.MinValue;
    }

}