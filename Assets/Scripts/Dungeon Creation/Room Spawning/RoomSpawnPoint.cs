using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawnPoint : MonoBehaviour
{
    [SerializeField]bool doNotSpawnHere;
    [SerializeField]bool debugMode;
    [SerializeField]bool SpawnBigRoom;
    private void Awake()
    {
        AddThisSpawnPointToDungeonCreatorList();
    }

    void AddThisSpawnPointToDungeonCreatorList()
    {
        if(doNotSpawnHere) return;

        if(SpawnBigRoom) RandomDungeonCreator.bigRoomSpawnPoints.Add(transform);
        else RandomDungeonCreator.smallRoomSpawnPoints.Add(transform);

        if (!debugMode) return;
        Debug.Log(gameObject.name + transform.position);
    }
}
