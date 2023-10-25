using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableSpawnPoint : MonoBehaviour
{
    [SerializeField] bool doNotSpawnHere;
    [SerializeField] bool debugMode;

    [SerializeField] bool isEnemySpawnPoint;

    private void Start()
    {
        AddThisSpawnPointToaDesignatedSpawnList();
    }

    void AddThisSpawnPointToaDesignatedSpawnList()
    {
        if(isEnemySpawnPoint)
        {
            RandomEnemySpawner.EnemySpawnPoints.Add(transform);
            return;
        }

        if (doNotSpawnHere) return;
        PickablesSpawnManager.AllPickableSpawnPoints.Add(transform);
        if (!debugMode) return;
        Debug.Log(gameObject.name + transform.position);
    }
}
