using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Perk : ScriptableObject
{
    public string perkName, perkDescription;
    public Sprite perkIcon;

    public virtual void ApplyPerk(PlayerController player)
    {
        
    }
}
