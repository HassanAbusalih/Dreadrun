using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack Speed Artifact Object", menuName = "ArtifactsEffects/Attack Speed Artifact")]
public class AttackSpeedArtifact : Artifact
{
    [SerializeField] float attackSpeedIncreasePerLevel;

    bool buffApplied = false;

    public float TotalAttackSpeedIncrease
    {
        get { return level * attackSpeedIncreasePerLevel; }
    }

    public override void ApplyArtifactBuffs(Vector3 artifactPosition, float effectRange, ArtifactManager Manager)
    {
        foreach (Player player in Manager.playersInGame)
        {
            if (Vector3.Distance(player.transform.position, artifactPosition) <= effectRange && buffApplied == false)
            {
                player.playerStats.attackSpeed += TotalAttackSpeedIncrease;
                buffApplied = true;
            }
            else if (Vector3.Distance(player.transform.position, artifactPosition) > effectRange && buffApplied == true)
            {
                player.playerStats.attackSpeed -= TotalAttackSpeedIncrease;
                buffApplied = false;
            }
        }
    }
}
