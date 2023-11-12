using System;
using UnityEngine;

[Serializable]
public class AttackArtifact : Artifact
{
    AttackArtifactSettings ArtifactSettings => (AttackArtifactSettings)base.settings;
    private float TotalAttackIncrease => level * ArtifactSettings.attackIncreasePerLevel;

    bool buffapplied = false;

    public override void Initialize()
    {
        this.prefab = ArtifactSettings.artifactPrefab;
    }

    public override void ApplyArtifactEffects()
    {
        foreach (Player player in manager.PlayersInGame)
        {
            if ((player.transform.position - manager.artifactPosition).sqrMagnitude <= manager.effectRange * manager.effectRange && !buffapplied)
            {
                if (buffapplied) return;
                player.GetComponent<Player>().playerStats.attackSpeed += TotalAttackIncrease;
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
        player.GetComponent<Player>().playerStats.attackSpeed += TotalAttackIncrease;
    }
}

