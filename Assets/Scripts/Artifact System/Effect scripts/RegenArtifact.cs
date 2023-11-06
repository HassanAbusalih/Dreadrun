using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Regen Artifact Object", menuName = "ArtifactsEffects/Regen Artifact")]
public class RegenArtifact : Artifact
{
    [SerializeField] float regenPerSecondPerLevel;

    float AttackStrength
    {
        get { return level * regenPerSecondPerLevel; }
    }

    public override void ApplyArtifactBuffs(Vector3 artifactPosition, float effectRange, ArtifactManager Manager)
    {
        foreach (Player player in Manager.playersInGame)
        {
            if (Vector3.Distance(player.transform.position, artifactPosition) <= effectRange)
            {
                Manager.StartCoroutine(RegenHealth(player));
            }
        }
    }

    IEnumerator RegenHealth(Player player)
    {
        while (true)
        {
            player.playerStats.health += AttackStrength;
            yield return new WaitForSeconds(1);
        }
    }
}
