using UnityEngine;

[CreateAssetMenu(fileName = "AttackSpeedArtifactSettings", menuName = "ArtifactSettings/AttackSpeedArtifactSettings")]
public class AttackSpeedArtifactSettings : ScriptableObject
{
    public GameObject artifactPrefab;
    public float attackSpeedIncreasePerLevel;
}
