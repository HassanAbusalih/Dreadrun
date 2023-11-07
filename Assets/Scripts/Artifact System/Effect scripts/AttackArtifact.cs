using UnityEngine;

[CreateAssetMenu(fileName = "Attack Artifact Object", menuName = "ArtifactsEffects/Attack Artifact")]
public class AttackArtifact : Artifact
{
    [SerializeField] float attackIncreasePerLevel;

    bool buffApplied = false;

    public float TotalAttackIncrease
    {
        get { return level * attackIncreasePerLevel; }
    }

    public override void ApplyArtifactBuffs(Vector3 artifactPosition, float effectRange, ArtifactManager Manager)
    {
        foreach (Player player in Manager.playersInGame)
        {
            if (Vector3.Distance(player.transform.position, artifactPosition) <= effectRange && buffApplied == false)
            {
                player.playerStats.attack += TotalAttackIncrease;
                buffApplied = true;
            }
            else if (Vector3.Distance(player.transform.position, artifactPosition) > effectRange && buffApplied == true)
            {
                player.playerStats.attack -= TotalAttackIncrease;
                buffApplied = false;
            }
        }
    }
}

