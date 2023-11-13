using System;
using UnityEngine;

public abstract class Artifact
{
    public GameObject gameObject;
    public int level;
    public ArtifactManager manager;
    public ArtifactSettings settings;

    ParticleSystem BuffVFX;
    ParticleSystem DeBuffVFX;

    public abstract void Initialize();
    public abstract void ApplyArtifactEffects();

    // Method to apply buffs. This reduces code duplication in Artifact classes that apply stat effects.
    protected void ApplyBuff(Player player, float effect, ref bool buffApplied, string fieldName)
    {
        // Get the PlayerStats field using reflection
        var field = typeof(PlayerStats).GetField(fieldName);

        // Get the current value of the field
        float currentValue = (float)field.GetValue(player.playerStats);

        if ((player.transform.position - manager.transform.position).sqrMagnitude <= manager.effectRange * manager.effectRange)
        {
            if(buffApplied) return;
            // Increase the field value by the effect
            field.SetValue(player.playerStats, currentValue + effect);
            BuffVFX = player.GetComponentsInChildren<ParticleSystem>()[0];
            BuffVFX.Play();
            buffApplied = true;
        }
        else if (buffApplied)
        {
            // Decrease the field value by the effect
            field.SetValue(player.playerStats, currentValue - effect);
            DeBuffVFX = player.GetComponentsInChildren<ParticleSystem>()[1];
            DeBuffVFX.Play();
            buffApplied = false;
        }
    }
}
