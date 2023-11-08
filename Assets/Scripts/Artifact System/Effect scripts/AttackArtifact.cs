using UnityEngine;

[CreateAssetMenu(fileName = "Attack Artifact Object", menuName = "ArtifactsEffects/Attack Artifact")]
public class AttackArtifact : Artifact
{
    [SerializeField] private float attackIncreasePerLevel;

    private bool buffApplied;

    private float TotalAttackIncrease => level * attackIncreasePerLevel;

    public override void InitializeArtifact() { }

    public override void ApplyArtifactBuffs(Vector3 artifactPosition, float effectRange, ArtifactManager manager)
    {
        foreach (Player player in manager.PlayersInGame)
        {
            bool isPlayerInRange = (player.transform.position - artifactPosition).sqrMagnitude <= effectRange * effectRange;

            if (isPlayerInRange && !buffApplied)
            {
                player.playerStats.attack += TotalAttackIncrease;
                buffApplied = true;
            }
            else if (!isPlayerInRange && buffApplied)
            {
                player.playerStats.attack -= TotalAttackIncrease;
                buffApplied = false;
            }
        }
    }
}

