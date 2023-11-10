using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack Speed Artifact Object", menuName = "ArtifactsEffects/Attack Speed Artifact")]
public class AttackSpeedArtifact : Artifact
{
    [SerializeField] private float attackSpeedIncreasePerLevel;

    private bool buffApplied;

    private float TotalAttackSpeedIncrease => level * attackSpeedIncreasePerLevel;

    public override void InitializeArtifact() { }

    public override void ApplyArtifactBuffs(Vector3 artifactPosition, float effectRange, ArtifactManager manager)
    {
        foreach (Player player in manager.PlayersInGame)
        {
            bool isPlayerInRange = (player.transform.position - artifactPosition).sqrMagnitude <= effectRange * effectRange;

            if (isPlayerInRange && !buffApplied)
            {
                player.playerStats.attackSpeed += TotalAttackSpeedIncrease;
                buffApplied = true;
            }
            else if (!isPlayerInRange && buffApplied)
            {
                player.playerStats.attackSpeed -= TotalAttackSpeedIncrease;
                buffApplied = false;
            }
        }
    }
}
