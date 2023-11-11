using UnityEngine;

[CreateAssetMenu(fileName = "ExplosiveArtifactSettings", menuName = "ArtifactSettings/ExplosiveArtifactSettings")]
public class ExplosiveArtifactSettings : ScriptableObject
{
    public GameObject artifactPrefab;
    public float explosionDamagePerLevel;
    public float pushBackForcePerLevel;
    public float explosionRadius;
    public int enemiesRequired;
    public int explosionCooldown;
}
