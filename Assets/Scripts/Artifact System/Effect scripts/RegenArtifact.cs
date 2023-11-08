using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Regen Artifact Object", menuName = "ArtifactsEffects/Regen Artifact")]
public class RegenArtifact : Artifact
{
    [SerializeField] private float regenPerSecondPerLevel;

    private float TotalHealthRegenPerSecond => level * regenPerSecondPerLevel;

    public override void InitializeArtifact() { }

    public override void ApplyArtifactBuffs(Vector3 artifactPosition, float effectRange, ArtifactManager manager)
    {
        foreach (Player player in manager.PlayersInGame)
        {
            if ((player.transform.position - artifactPosition).sqrMagnitude <= effectRange * effectRange)
            {
                manager.StartCoroutine(RegenHealth(player));
            }
        }
    }

    private IEnumerator RegenHealth(Player player)
    {
        while (true)
        {
            player.playerStats.health += TotalHealthRegenPerSecond;
            yield return new WaitForSeconds(1);
        }
    }
}
