using UnityEngine;

[CreateAssetMenu(fileName = "AttackArtifactSettings", menuName = "ArtifactSettings/AttackArtifactSettings")]
public class AttackArtifactSettings : ScriptableObject
{
    public GameObject artifactPrefab;
    public float attackIncreasePerLevel;
}
