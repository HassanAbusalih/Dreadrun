using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    private void Start()
    {
        RandomDungeonCreator.dungeonSpawnPoints.Add(transform);
        Debug.Log(RandomDungeonCreator.dungeonSpawnPoints.Count);
    }


}
