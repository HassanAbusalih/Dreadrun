using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;



public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] GameObject payload;
    [SerializeField] GameObject player;
    public GameObject prefabToSpawn;
    public float spawnRadius;
    public static float timeBetweenSpawns = 3f;
    public bool spawnInside;
    public bool limitSpawnAmount = false;
    private int spawnCount = 0;
    public int maxSpawnCount = 10;


   
   
    private void Start()
    {
        StartCoroutine(StartSpawning());
    }
 

    private IEnumerator StartSpawning()
    {
        while (true)
        {
          

            if (limitSpawnAmount)
            {
                SpawnObjectWithMax();
                yield return new WaitForSeconds(timeBetweenSpawns);
            }
            else
            {
                SpawnObject();
                yield return new WaitForSeconds(timeBetweenSpawns);
            }

        }
    }

    public  void SpawnObject()
    {
        Vector3 spawnPosition = SpawnPostionForObject() * spawnRadius + transform.position;
        GameObject newObject = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
        newObject.transform.parent = transform;
        ProvideReferenceToObjectSpawned(newObject, true);
    }


    Vector3  SpawnPostionForObject()
    {
        Vector3 outsideRandomPoition = new Vector3(Random.insideUnitCircle.x, 0, Random.insideUnitCircle.y).normalized ;
        Vector3 insiderandomPosition = new Vector3(Random.insideUnitCircle.x, 0, Random.insideUnitCircle.y) ;
        return spawnInside ? insiderandomPosition  : outsideRandomPoition ;
    }

    void ProvideReferenceToObjectSpawned( GameObject gameObject, bool giveReference )
    {
        if(giveReference)
        {
            if (gameObject.TryGetComponent(out PlaceholderEnemyAI enemy))
            {
                enemy.player = player.transform;
                enemy.payload = payload.transform;
            }
        }
    }

    private void SpawnObjectWithMax()
    {
        if (spawnCount < maxSpawnCount)
        {
            SpawnObject();
            spawnCount++;
        }
        else
        {
            CancelInvoke("SpawnObjectWithMax");
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);

        Gizmos.color = new Color(0, 0, 0, 0.2f);
        Gizmos.DrawSphere(transform.position, spawnRadius);

        Handles.color = Color.black;
        Handles.DrawWireDisc(transform.position, Vector3.up, spawnRadius, 5f);

        Handles.color = new Color(1, 0, 0, 0.2f);
        Handles.DrawSolidDisc(transform.position, Vector3.up, spawnRadius);

        SetTextAndStyle(gameObject.name, Vector3.up, FontStyle.Normal, Color.black);
    }
    private void  OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 0, 1, 0.2f);
        Gizmos.DrawSphere(transform.position,spawnRadius);

        SetTextAndStyle(gameObject.name, Vector3.up, FontStyle.BoldAndItalic, Color.blue);
    }
    void SetTextAndStyle(string textContext,Vector3 offsetPosition,FontStyle style, Color textColor)
    {
        GUIStyle labelStyle = new GUIStyle();
        labelStyle.normal.textColor = textColor;
        labelStyle.alignment = TextAnchor.MiddleCenter;
        labelStyle.fontStyle = style;
        Handles.Label(transform.position + offsetPosition, textContext, labelStyle);
    }
}

[CustomEditor(typeof(ObjectSpawner))]
public class ObjectSpawnerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        ObjectSpawner spawner = (ObjectSpawner)target;

        spawner.prefabToSpawn = (GameObject)EditorGUILayout.ObjectField("Prefab to Spawn", spawner.prefabToSpawn, typeof(GameObject), true);
        spawner.spawnRadius = EditorGUILayout.FloatField("Spawn Radius", spawner.spawnRadius);
        ObjectSpawner.timeBetweenSpawns = EditorGUILayout.FloatField("TimeBetweenSpawns", ObjectSpawner.timeBetweenSpawns);
        spawner.spawnInside = EditorGUILayout.Toggle("Spawn Inside Circle", spawner.spawnInside);
        spawner.limitSpawnAmount = EditorGUILayout.Toggle("Limit Spawning", spawner.limitSpawnAmount);

        if (spawner.limitSpawnAmount)
        {
            spawner.maxSpawnCount = EditorGUILayout.IntField("Max Spawn Count", spawner.maxSpawnCount);
        }

        if (GUILayout.Button("Spawn Object Now"))
        {
            spawner.SpawnObject();
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
}

