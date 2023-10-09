using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    private void Awake()
    {
        RandomDungeonCreator.dungeonSpawnPoints.Add(transform);

        Debug.Log(gameObject.name + transform.position);
    }


}
