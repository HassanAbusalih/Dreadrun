using System;
using UnityEngine;

[Serializable]
public class AttackArtifact : Artifact
{
    AttackArtifactSettings ArtifactSettings => (AttackArtifactSettings)base.settings;
    private float TotalAttackIncrease => level * ArtifactSettings.attackIncreasePerLevel;

    public override void ApplyArtifactEffects()
    {
        foreach (Player player in manager.PlayersInGame)
        {
            if ((player.transform.position - manager.artifactPosition).sqrMagnitude <= manager.effectRange * manager.effectRange)
                player.GetComponent<Player>().playerStats.attackSpeed += TotalAttackIncrease;
            else
                RemoveArtifactEffects(player);
        }
    }

    void RemoveArtifactEffects(Player player)
    {
        player.GetComponent<Player>().playerStats.attackSpeed += TotalAttackIncrease;
    }
}

