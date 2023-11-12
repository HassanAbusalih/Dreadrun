using System;
using UnityEngine;

[Serializable]
public class AttackSpeedArtifact : Artifact
{
    AttackSpeedArtifactSettings ArtifactSettings => (AttackSpeedArtifactSettings)base.settings;
    private float TotalAttackSpeedIncrease => level * ArtifactSettings.attackSpeedIncreasePerLevel;
    
    bool buffapplied = false;

    public override void Initialize()
    {
        this.prefab = ArtifactSettings.artifactPrefab;
    }

    public override void ApplyArtifactEffects()
    {
        foreach (Player player in manager.PlayersInGame)
        {
            if ((player.transform.position - manager.artifactPosition).sqrMagnitude <= manager.effectRange * manager.effectRange)
            {
                if (buffapplied) return;
                player.GetComponent<Player>().playerStats.attackSpeed += TotalAttackSpeedIncrease;
                buffapplied = true;
            }
            else if (buffapplied)
            {
                RemoveArtifactEffects(player);
                buffapplied = false;
            }
        }
    }

    void RemoveArtifactEffects(Player player)
    {
        player.playerStats.attackSpeed -= TotalAttackSpeedIncrease;
    }
}
