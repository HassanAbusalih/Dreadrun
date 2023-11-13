using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RegenArtifact : Artifact
{
    private Dictionary<Player, Coroutine> regenCoroutines = new Dictionary<Player, Coroutine>();

    // Cast the value of inherited settings variable to the correct settings type and assign it to a variable for easier use
    RegenArtifactSettings ArtifactSettings => (RegenArtifactSettings)base.settings;
    private float TotalHealthRegenPerSecond => level * ArtifactSettings.regenPerSecondPerLevel;

    bool buffapplied = false;

    public override void Initialize()
    {
        this.gameObject = ArtifactSettings.artifactPrefab;
    }

    public override void ApplyArtifactEffects()
    {
        foreach (Player player in manager.PlayersInGame)
        {
            if ((player.transform.position - manager.transform.position).sqrMagnitude <= manager.effectRange * manager.effectRange)
            {
                if (!regenCoroutines.ContainsKey(player))
                {
                    StartHealthRegen(player);
                }
            }
            else if (regenCoroutines.ContainsKey(player))
            {
                StopHealthRegen(player);
            }
        }
    }

    void StopHealthRegen(Player player)
    {
        manager.StopCoroutine(regenCoroutines[player]);
        regenCoroutines.Remove(player);
    }

    void StartHealthRegen(Player player)
    {
        regenCoroutines.Add(player, manager.StartCoroutine(RegenHealth(player)));
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
