using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableSpawnPoint : MonoBehaviour
{
    [SerializeField] bool doNotSpawnHere;
    [SerializeField] bool debugMode;
    private void Awake()
    {
        AddThisSpawnPointToPickablesSpawnPointsList();
    }

    void AddThisSpawnPointToPickablesSpawnPointsList()
    {
        if (doNotSpawnHere) return;
        PickablesSpawnManager.AllPickableSpawnPoints.Add(transform);
        if (!debugMode) return;
        Debug.Log(gameObject.name + transform.position);
    }
}
