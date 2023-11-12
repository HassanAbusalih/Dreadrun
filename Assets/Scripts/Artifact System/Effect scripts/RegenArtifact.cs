using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RegenArtifact : Artifact
{
    private Dictionary<Player, Coroutine> regenCoroutines;

    RegenArtifactSettings ArtifactSettings => (RegenArtifactSettings)base.settings;
    private float TotalHealthRegenPerSecond => level * ArtifactSettings.regenPerSecondPerLevel;

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
                regenCoroutines.Add(player, manager.StartCoroutine(RegenHealth(player)));
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
        manager.StopCoroutine(regenCoroutines[player]);
    }

    private IEnumerator RegenHealth(Player player)
    {
        while (player.playerStats.health < player.playerStats.maxHealth)
        {
            player.playerStats.health += TotalHealthRegenPerSecond;
            yield return new WaitForSeconds(1);
        }
    }
}
