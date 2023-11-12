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
    [SerializeField] private GameObject artifactSpawnPoint;
    [SerializeField] private int minArtifactLevel;
    [SerializeField] private int maxArtifactLevel;
    [SerializeField] private bool useCustomEffectRange;

    private Artifact currentArtifact;

    private void Start()
    {
        GameManager.Instance.onPhaseChange.AddListener(EnableArtifactSystem);
        PlayersInGame = FindObjectsOfType<Player>();
        enemyLayer = LayerMask.GetMask("Enemy");

        if (currentArtifact == null)
        {
            currentArtifact = SpawnNewArtifact();
            artifactPosition = artifactGameObject.transform.position;
            Debug.Log(currentArtifact);
        }

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
        Artifact[] artifactInstances = CreateArtifactInstances();
        currentArtifact = artifactInstances[2];
        artifactGameObject = Instantiate(currentArtifact.prefab, artifactSpawnPoint.transform.position, Quaternion.identity, artifactSpawnPoint.transform);
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
            artifact.manager = this;
            artifact.Initialize();
        }

        return artifactInstances;
    }

    private void EnableArtifactSystem()
    {
        gameObject.SetActive(true);
    }
}
