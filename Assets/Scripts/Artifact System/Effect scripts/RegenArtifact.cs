using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Regen Artifact Object", menuName = "ArtifactsEffects/Regen Artifact")]
public class RegenArtifact : Artifact
{
    [SerializeField] private float regenPerSecondPerLevel;

    private float TotalHealthRegenPerSecond => level * regenPerSecondPerLevel;
    private Coroutine regenRoutine = null;

    public override void InitializeArtifact()
    {
        regenRoutine = null;
    }

    public override void ApplyArtifactBuffs(Vector3 artifactPosition, float effectRange, ArtifactManager manager)
    {
        foreach (Player player in manager.PlayersInGame)
        {
            if ((player.transform.position - artifactPosition).sqrMagnitude <= effectRange * effectRange)
            {
                regenRoutine = manager.StartCoroutine(RegenHealth(player));
            }
            else
            {
                if (regenRoutine != null)
                    manager.StopCoroutine(regenRoutine);
            }
        }
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
