using System;
using System.Linq;
using UnityEngine;
using System.Reflection;
using Random = UnityEngine.Random;

public class ArtifactManager : MonoBehaviour
{
    [NonSerialized] public LayerMask enemyLayer;
    [NonSerialized] public Player[] PlayersInGame;
    [NonSerialized] public GameObject artifactGameObject;
    [NonSerialized] public Vector3 artifactPosition;
    [ConditionalHide("useCustomEffectRange", true)] public float effectRange;

    [SerializeField] private ScriptableObject[] artifactSettings;
    [SerializeField] private Artifact[] artifactTypes;
    [SerializeField] private GameObject artifactSpawnPoint;
    [SerializeField] private int minArtifactLevel;
    [SerializeField] private int maxArtifactLevel;
    [SerializeField] private bool useCustomEffectRange;

    private Artifact currentArtifact;

    private void Start()
    {
        GameManager.Instance.onPhaseChange.AddListener(EnableArtifactSystem);
        PlayersInGame = FindObjectsOfType<Player>();
        artifactTypes = CreateArtifactInstances();
        enemyLayer = LayerMask.GetMask("Enemy");

        if (currentArtifact == null)
        {
            currentArtifact = SpawnNewArtifact();
            currentArtifact.manager = this;
        }

        artifactPosition = artifactGameObject.transform.position;
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!useCustomEffectRange)
            effectRange = PayloadStats.instance.payloadRange;

        currentArtifact.ApplyArtifactEffects();
    }

    private Artifact SpawnNewArtifact()
    {
        currentArtifact = artifactTypes[Random.Range(0, artifactTypes.Length)];
        artifactGameObject = Instantiate(currentArtifact.prefab ?? GameObject.CreatePrimitive(PrimitiveType.Sphere), artifactSpawnPoint.transform);
        currentArtifact.level = Random.Range(minArtifactLevel, maxArtifactLevel);
        return currentArtifact;
    }

    private Artifact[] CreateArtifactInstances()
    {
        Assembly assembly = Assembly.GetExecutingAssembly();
        Type[] artifactTypes = assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(Artifact)) && !t.IsAbstract).ToArray();
        Artifact[] artifactInstances = artifactTypes.Select(t => (Artifact)Activator.CreateInstance(t)).ToArray();

        foreach (Artifact artifact in artifactInstances)
        {
            artifact.settings = artifactSettings.FirstOrDefault(s => s.GetType().Name == artifact.GetType().Name + "Settings");
        }

        return artifactInstances;
    }

    private void EnableArtifactSystem()
    {
        gameObject.SetActive(true);
    }
}
