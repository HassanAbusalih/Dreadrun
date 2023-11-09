using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class ArtifactManager : MonoBehaviour
{
    [SerializeField] GameObject artifactSpawnPoint;
    [SerializeField] Artifact[] artifacts;
    [SerializeField] float effectRange;
    [SerializeField] int minArtifactLevel;
    [SerializeField] int maxArtifactLevel;
    [NonSerialized] public Player[] playersInGame;
    private Artifact currentArtifact;
    private GameObject artifactGameObject;

    private void Start()
    {
        playersInGame = FindObjectsOfType<Player>();

        if (currentArtifact == null)
        {
            currentArtifact = SpawnArtifact();
        }
    }

    private void Update()
    {
        effectRange = PayloadStats.instance.payloadRange;

        if (currentArtifact == null)
        {
            currentArtifact = SpawnArtifact();
        }
        else
        {
            currentArtifact.ApplyArtifactBuffs(artifactGameObject.transform.position, effectRange, this);
        }
    }

    Artifact SpawnArtifact()
    {
        int artifactIndex = Random.Range(0, artifacts.Length);
        currentArtifact = artifacts[artifactIndex];

        if (currentArtifact.prefab != null)
        {
            artifactGameObject = Instantiate(currentArtifact.prefab, artifactSpawnPoint.transform);
        }
        else
        {
            artifactGameObject = Instantiate(GameObject.CreatePrimitive(PrimitiveType.Sphere), artifactSpawnPoint.transform);
        }

        currentArtifact.level = Random.Range(minArtifactLevel, maxArtifactLevel + 1);

        return currentArtifact;
    }
}
