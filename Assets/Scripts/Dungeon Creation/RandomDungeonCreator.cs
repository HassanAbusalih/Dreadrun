using System;
using System.Collections.Generic;
using UnityEngine;


public class RandomDungeonCreator : MonoBehaviour
{

    public static List<Transform> dungeonSpawnPoints = new List<Transform>();  
    [SerializeField] GameObject[] roomPrefabs;
    [SerializeField] int numberOfRoomsToSpawn;


    private void OnValidate()
    {
        if (numberOfRoomsToSpawn > roomPrefabs.Length )
        {
            numberOfRoomsToSpawn = 0;
        }
    }


 

}
