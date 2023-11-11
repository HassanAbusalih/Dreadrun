using UnityEngine;

[CreateAssetMenu(fileName = "RegenArtifactSettings", menuName = "ArtifactSettings/RegenArtifactSettings")]
public class RegenArtifactSettings : ScriptableObject
{
    public GameObject artifactPrefab;
    public float regenPerSecondPerLevel;
}
