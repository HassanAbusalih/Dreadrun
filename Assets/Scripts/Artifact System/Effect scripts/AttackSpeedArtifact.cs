using System;
using UnityEngine;

[Serializable]
public class AttackSpeedArtifact : Artifact
{
    // Cast the value of inherited settings variable to the correct settings type and assign it to a variable for easier use
    AttackSpeedArtifactSettings ArtifactSettings => (AttackSpeedArtifactSettings)base.settings;
    private float TotalAttackSpeedIncrease => level * ArtifactSettings.attackSpeedIncreasePerLevel;
    
    bool buffapplied = false;

    public override void Initialize()
    {
     
    }

    public override void ApplyArtifactEffects()
    {
        foreach (Player player in manager.PlayersInGame)
        {
            ApplyBuff(player, TotalAttackSpeedIncrease, ref buffapplied, "attackSpeed");
        }
    }
}
