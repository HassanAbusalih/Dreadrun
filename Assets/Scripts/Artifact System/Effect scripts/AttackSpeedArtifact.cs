using System;
using UnityEngine;

[Serializable]
public class AttackSpeedArtifact : Artifact
{
    AttackSpeedArtifactSettings ArtifactSettings => (AttackSpeedArtifactSettings)base.settings;
    private float TotalAttackSpeedIncrease => level * ArtifactSettings.attackSpeedIncreasePerLevel;

    public override void ApplyArtifactEffects()
    {
        foreach (Player player in manager.PlayersInGame)
        {
            if ((player.transform.position - manager.artifactPosition).sqrMagnitude <= manager.effectRange * manager.effectRange)
                player.GetComponent<Player>().playerStats.attackSpeed += TotalAttackSpeedIncrease;
            else
                RemoveArtifactEffects(player);
        }
    }

    void RemoveArtifactEffects(Player player)
    {
        player.playerStats.attackSpeed -= TotalAttackSpeedIncrease;
    }
}
