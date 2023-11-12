using System;
using System.Linq;
using UnityEngine;
using System.Reflection;
using Random = UnityEngine.Random;
using Unity.VisualScripting;

public class ArtifactManager : MonoBehaviour
{
    // Variables used by artifact scripts
    [Header("Artifact effect variables")]
    [NonSerialized] public Player[] PlayersInGame;
    [NonSerialized] public GameObject artifactGameObject;
    [NonSerialized] public Vector3 artifactPosition;
    [NonSerialized] public LayerMask enemyLayer;
    // ConditionalHide hides the field in the inspector if the condition is not met
    [ConditionalHide("useCustomEffectRange", true)] public float effectRange;

    // Variables only used by artifact manager
    [Header("Artifact manager variables")]
    [SerializeField] private ArtifactSettings[] artifactSettings;
    [SerializeField] private GameObject artifactSpawnPoint;
    [SerializeField] private int minArtifactLevel;
    [SerializeField] private int maxArtifactLevel;
    [SerializeField] private bool useCustomEffectRange;
    private Artifact currentArtifact;

    private void Start()
    {
        GameManager.Instance.onPhaseChange.AddListener(EnableArtifactSystem);

        /* Get all players in the game and the enemy layer to prevent code duplication in artifact scripts,
        might refactor this as the payload scripts also use this */
        PlayersInGame = FindObjectsOfType<Player>();
        enemyLayer = LayerMask.GetMask("Enemy");

        // If there is no current artifact, spawn a new one
        if (currentArtifact == null)
        {
            SpawnNewArtifact();
            artifactPosition = artifactGameObject.transform.position;
            Debug.Log(currentArtifact);
        }

        gameObject.SetActive(false);
    }

    private void Update()
    {
        // If not using a custom effect range, set the effect range to the payload range
        if (!useCustomEffectRange)
            effectRange = PayloadStats.instance.payloadRange;

        // Apply the artifact effects if there is a current artifact
        currentArtifact?.ApplyArtifactEffects();
    }

    private void SpawnNewArtifact()
    {
        // Get a random artifact
        currentArtifact = GetRandomArtifact();
        // Instantiate the artifact game object at the spawn point
        artifactGameObject = Instantiate(currentArtifact.settings.artifactPrefab,
            artifactSpawnPoint.transform.position, Quaternion.identity,
            artifactSpawnPoint.transform);
        // Set the game object of the current artifact
        currentArtifact.gameObject = artifactGameObject;
        // Initialize the current artifact
        currentArtifact.Initialize();
        // Set the level of the current artifact
        currentArtifact.level = Random.Range(minArtifactLevel, maxArtifactLevel);
    }

    private Artifact GetRandomArtifact()
    {
        // Get the executing assembly
        Assembly assembly = Assembly.GetExecutingAssembly();
        // Get all types that are subclasses of Artifact and are not abstract
        Type[] artifactTypes = assembly.GetTypes().Where
            (t => t.IsSubclassOf(typeof(Artifact)) && !t.IsAbstract).ToArray();
        // Create an instance of a ranom artifact type
        Artifact artifactInstance = (Artifact)Activator.CreateInstance(artifactTypes[Random.Range(0, artifactTypes.Length)]);
        // Set the settings of the artifact instance
        artifactInstance.settings = artifactSettings.FirstOrDefault
            (s => s.GetType().Name == artifactInstance.GetType().Name + "Settings");
        // Set the manager of the artifact instance
        artifactInstance.manager = this;

        // Return the artifact instance
        return artifactInstance;
    }


    private void EnableArtifactSystem()
    {
        gameObject.SetActive(true);
    }
}
