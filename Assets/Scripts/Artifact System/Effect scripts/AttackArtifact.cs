using System;

[Serializable]
public class AttackArtifact : Artifact
{
    // Cast the value of inherited settings variable to the correct settings type and assign it to a variable for easier use
    AttackArtifactSettings ArtifactSettings => (AttackArtifactSettings)base.settings;
    private float TotalAttackIncrease => level * ArtifactSettings.attackIncreasePerLevel;

    bool buffapplied = false;

    public override void Initialize()
    {
        
    }

    public override void ApplyArtifactEffects()
    {
        foreach (Player player in manager.PlayersInGame)
        {
            if (ApplyBuff(player, TotalAttackIncrease, ref buffapplied, "attack")) break;
        }
    }
}

