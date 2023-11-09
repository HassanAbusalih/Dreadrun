using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class ArtifactManager : MonoBehaviour
{
    [SerializeField] private GameObject artifactSpawnPoint;
    [SerializeField] private Artifact[] artifacts;
    [SerializeField] private float effectRange;
    [SerializeField] private int minArtifactLevel;
    [SerializeField] private int maxArtifactLevel;
    [NonSerialized] public Player[] PlayersInGame;
    private Artifact currentArtifact;
    private GameObject artifactGameObject;

    private void Start()
    {
        GameManager.Instance.onPhaseChange.AddListener(EnableArtifactSystem);

        PlayersInGame = FindObjectsOfType<Player>();

        if (currentArtifact == null)
        {
            currentArtifact = SpawnNewArtifact();
        }

        currentArtifact.InitializeArtifact();

        gameObject.SetActive(false);
    }

    private void Update()
    {
        effectRange = PayloadStats.instance.payloadRange;

        if (currentArtifact == null)
        {
            currentArtifact = SpawnNewArtifact();
        }
        else
        {
            currentArtifact.ApplyArtifactBuffs(artifactGameObject.transform.position, effectRange, this);
        }
    }

    private Artifact SpawnNewArtifact()
    {
        int artifactIndex = Random.Range(0, artifacts.Length);
        currentArtifact = artifacts[artifactIndex];
        if (currentArtifact.prefab == null)
        {
            artifactGameObject = Instantiate(GameObject.CreatePrimitive(PrimitiveType.Sphere), artifactSpawnPoint.transform);
        }
        else
        {
            artifactGameObject = Instantiate(currentArtifact.prefab, artifactSpawnPoint.transform);
        }

        currentArtifact.level = Random.Range(minArtifactLevel, maxArtifactLevel);

        return currentArtifact;
    }

    private void EnableArtifactSystem()
    {
        gameObject.SetActive(true);
    }
}
