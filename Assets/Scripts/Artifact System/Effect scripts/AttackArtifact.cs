using UnityEngine;

[CreateAssetMenu(fileName = "Attack Artifact Object", menuName = "ArtifactsEffects/Attack Artifact")]
public class AttackArtifact : Artifact
{
    [SerializeField] float attackIncreasePerLevel;

    bool buffApplied = false;

    public float AttackIncreaseBy
    {
        get { return level * attackIncreasePerLevel; }
    }

    public override void ApplyArtifactBuffs(Vector3 artifactPosition, float effectRange, ArtifactManager Manager)
    {
        foreach (Player player in Manager.playersInGame)
        {
            if (Vector3.Distance(player.transform.position, artifactPosition) <= effectRange && buffApplied == false)
            {
                player.playerStats.attack += AttackIncreaseBy;
                buffApplied = true;
            }
            else if (Vector3.Distance(player.transform.position, artifactPosition) > effectRange && buffApplied == true)
            {
                player.playerStats.attack -= AttackIncreaseBy;
                buffApplied = false;
            }
        }
    }
}

