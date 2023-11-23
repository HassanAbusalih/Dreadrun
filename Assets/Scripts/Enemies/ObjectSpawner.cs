using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] GameObject prefabToSpawn;
    [SerializeField] float activationRadius;
    [SerializeField] float timeBetweenBatches = 3f;
    [SerializeField] int batchSize = 5;
    [SerializeField] int maxSpawnCount = 10;
    [SerializeField] Transform spawnPosition;
    [SerializeField] Vector3 spawnOffset = new(1f, 0, 1f);
    Transform payload;
    Transform[] players;
    Coroutine spawning;
    int spawnCount = 0;

    void Start()
    {
        if (spawnPosition == null) { spawnPosition = transform; }
        payload = FindObjectOfType<Payload>()?.transform;
        Player[] players = FindObjectsOfType<Player>();
        this.players = new Transform[players.Length];
        for (int i = 0; i < players.Length; i++)
        {
            this.players[i] = players[i].transform;
        }
        spawnCount = 0;
    }

    void Update()
    {
        if (payload != null && spawning == null && ActivateSpawner())
        {
            spawning = StartCoroutine(Spawning());
        }
    }

    IEnumerator Spawning()
    {
        while (spawnCount < maxSpawnCount)
        {
            for (int i = 0; i < batchSize; i++)
            {
                if (spawnCount >= maxSpawnCount)
                {
                    yield break;
                }
                SpawnObject();
                yield return new WaitForSeconds(0.2f);
            }
            yield return new WaitForSeconds(timeBetweenBatches);
        }
    }

    void SpawnObject()
    {
        Vector3 position = transform.position;
        if (spawnPosition != null) { position = spawnPosition.position; }
        Vector3 randomOffset = new(Random.Range(-spawnOffset.x, spawnOffset.x), 0, Random.Range(-spawnOffset.z, spawnOffset.z));
        GameObject newObject = Instantiate(prefabToSpawn, position + randomOffset, Quaternion.identity, transform);
        if (newObject.TryGetComponent(out EnemyAIBase enemy))
        {
            enemy.Initialize(payload, players);
        }
        else
        {
            Debug.Log("lalallalalala");
        }
        spawnCount++;
    }

    public void TestSpawner() => StartCoroutine(Testing());

    IEnumerator Testing()
    {
        yield return Spawning();
        spawnCount = 0;
    }

    bool ActivateSpawner()
    {
        if (Vector3.Distance(transform.position, payload.position) <= activationRadius)
        {
            return true;
        }
        foreach (var player in players)
        {
            if (Vector3.Distance(transform.position, player.position) <= activationRadius)
            {
                return true;
            }
        }
        return false;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, activationRadius);
        Gizmos.color = new Color(0, 0, 0, 0.2f);
        Gizmos.DrawSphere(transform.position, activationRadius);
        Handles.color = new Color(1, 0, 0, 0.2f);
        Handles.DrawSolidDisc(transform.position, Vector3.up, activationRadius);

        if (spawnPosition != null)
        {
            Gizmos.color = new Color(0, 1, 0, 0.2f);
            Gizmos.DrawSphere(spawnPosition.position, spawnOffset.magnitude);
            Gizmos.color = new Color(0, 0, 0, 0.5f);
            Gizmos.DrawWireSphere(spawnPosition.position, spawnOffset.magnitude);
        }

        GUIStyle labelStyle = new();
        labelStyle.normal.textColor = Color.black;
        labelStyle.alignment = TextAnchor.MiddleCenter;
        labelStyle.fontStyle = FontStyle.Normal;
        Handles.Label(transform.position + Vector3.up, gameObject.name, labelStyle);
    }
#endif
}

#if UNITY_EDITOR
[CustomEditor(typeof(ObjectSpawner))]
public class ObjectSpawnerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        ObjectSpawner spawner = (ObjectSpawner)target;
        if (GUILayout.Button("Test Spawning"))
        {
            spawner.TestSpawner();
        }
        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
}
#endif