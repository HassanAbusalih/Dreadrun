using System;
using System.Collections.Generic;
using UnityEngine;


public class RandomDungeonCreator : MonoBehaviour
{
    public static List<Transform> dungeonSpawnPoints = new List<Transform>();

    [SerializeField] int numberOfRoomsToSpawn;
    [SerializeField] int roomPrefabToSkipAfterFirstSpawnIteration;
    [SerializeField] bool destroySpawnPointParentOnSpawn;
    [SerializeField] GameObject[] roomPrefabs;



  
    private void Start()
    {    
        dungeonSpawnPoints.RemoveAll(item => item == null);

        StartTheRandomRoomSpawningProcess();
    }
  
    void StartTheRandomRoomSpawningProcess()
    {
       
        SpawnRoomsRandomly();
    }

    void SpawnRoomsRandomly()
    {
        int roomToSpawnIndex = 0;
        for (int i = 0; i < numberOfRoomsToSpawn; i++)
        {

            if (dungeonSpawnPoints.Count ==0) { Debug.LogError("Not enough spawn points to spawn rooms"); return; }


            int _randomSpawnPointIndex = UnityEngine.Random.Range(0, dungeonSpawnPoints.Count);
            ResetRoomPrefabsToSpawnIndex(ref roomToSpawnIndex);

            GetRoomToSpawnTransformData(roomToSpawnIndex, _randomSpawnPointIndex, out GameObject _roomToSpawn,
                                                out Vector3 _roomSpawnPosition, out Vector3 _roomRotation);

            GameObject _roomSpawned = Instantiate(_roomToSpawn, _roomSpawnPosition, Quaternion.Euler(_roomRotation));
            DestroyAndRemoveSpawnPoint(_randomSpawnPointIndex);
            roomToSpawnIndex++;
        }
    }
    int ResetRoomPrefabsToSpawnIndex(ref int _currentRoomToSpawnIndex)
    {
        if (_currentRoomToSpawnIndex == roomPrefabs.Length)
        {
            _currentRoomToSpawnIndex = 0 + roomPrefabToSkipAfterFirstSpawnIteration;
            return _currentRoomToSpawnIndex;
        }
        else { return _currentRoomToSpawnIndex; }
    }

    void GetRoomToSpawnTransformData(int _index, int _randomSpawnPointIndex, out GameObject _roomToSpawn, out Vector3 _roomSpawnPosition, out Vector3 _roomRotation)
    {
        _roomToSpawn = roomPrefabs[_index];
        _roomSpawnPosition = dungeonSpawnPoints[_randomSpawnPointIndex].position;
        _roomSpawnPosition.y = dungeonSpawnPoints[_randomSpawnPointIndex].root.position.y;
        _roomRotation = dungeonSpawnPoints[_randomSpawnPointIndex].eulerAngles;
    }

    private void DestroyAndRemoveSpawnPoint(int _randomSpawnPointIndex)
    {
        if (destroySpawnPointParentOnSpawn)
        {
            Destroy(dungeonSpawnPoints[_randomSpawnPointIndex].gameObject.transform.parent.gameObject);
        }
        else
        {
            Destroy(dungeonSpawnPoints[_randomSpawnPointIndex].gameObject);
        }

        dungeonSpawnPoints.RemoveAt(_randomSpawnPointIndex);
    }
}
