using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawnPoint : MonoBehaviour
{
    [SerializeField]bool doNotSpawnHere;
    [SerializeField]bool debugMode;
    private void Awake()
    {
        AddThisSpawnPointToDungeonCreatorList();
    }

    void AddThisSpawnPointToDungeonCreatorList()
    {
        if (doNotSpawnHere) return;
        RandomDungeonCreator.dungeonSpawnPoints.Add(transform);
        if (!debugMode) return;
        Debug.Log(gameObject.name + transform.position);
    }




}
