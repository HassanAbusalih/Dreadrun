using System;
using System.Collections.Generic;
using UnityEngine;


public class RandomDungeonCreator : MonoBehaviour
{

    public static List<Transform> dungeonSpawnPoints = new List<Transform>();
    [SerializeField] int numberOfRoomsToSpawn;
    [SerializeField] GameObject[] roomPrefabs;
    

    [SerializeField] List<GameObject> roomsSpawned;


    private void OnValidate()
    {
        if (numberOfRoomsToSpawn > roomPrefabs.Length )
        {
            numberOfRoomsToSpawn = 0;
        }
    }

    private void Start()
    {
       RandomlyGenerateRoomsOnSpawnPoints();
    }
    private void Update()
    {
       
    }

    void RandomlyGenerateRoomsOnSpawnPoints()
    {
        for (int i = 0; i < numberOfRoomsToSpawn; i++)
        {
            int _randomSpawnPointIndex = UnityEngine.Random.Range(0,dungeonSpawnPoints.Count);

            GameObject _roomToSpawn = roomPrefabs[i];
            Vector3 _roomSpawnPosition = dungeonSpawnPoints[_randomSpawnPointIndex].position;
            Quaternion _roomRotation  = dungeonSpawnPoints[_randomSpawnPointIndex].localRotation;
           
            _roomSpawnPosition.y = 0;

            GameObject _roomSpawned = Instantiate(_roomToSpawn, _roomSpawnPosition, _roomRotation);

            Destroy(dungeonSpawnPoints[_randomSpawnPointIndex].gameObject.transform.parent.gameObject);
            dungeonSpawnPoints.RemoveAt(_randomSpawnPointIndex);
        }
    }




}
