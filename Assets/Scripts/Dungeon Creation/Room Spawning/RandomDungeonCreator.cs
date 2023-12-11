using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class RandomDungeonCreator : MonoBehaviour
{

    public static List<Transform> smallRoomSpawnPoints = new List<Transform>();
    [Header("Small Rooms Spawn Settings")]
    [SerializeField] int minSmallRoomsToSpawn;
    [SerializeField] int maxSmallRoomsToSpawn;
    [SerializeField] GameObject[] smallRoomPrefabs;

    public static List<Transform> bigRoomSpawnPoints = new List<Transform>();
    [Header("Big Rooms Spawn Settings")]
    [SerializeField] int minBigRoomsToSpawn;
    [SerializeField] int maxBigRoomsToSpawn;
    [SerializeField] GameObject[] bigRoomPrefabs;

    [Header("Debug Stats")]
    [SerializeField] int smallRoomsSpawned;
    [SerializeField] int bigRoomsSpawned;
    public int totalRoomsSpawned;

    [SerializeField] bool destroySpawnPointParentOnSpawn;
    

    private void Awake()
    {
        smallRoomSpawnPoints.Clear();
        bigRoomSpawnPoints.Clear();
    }

    private void Start()
    {
        StartRandomRoomSpawningProcess();
        totalRoomsSpawned = smallRoomsSpawned + bigRoomsSpawned;
    }

    void StartRandomRoomSpawningProcess()
    {
        SpawnRooms(ref bigRoomsSpawned,minBigRoomsToSpawn, maxBigRoomsToSpawn, ref bigRoomSpawnPoints, bigRoomPrefabs);
        SpawnRooms(ref smallRoomsSpawned,minSmallRoomsToSpawn, maxSmallRoomsToSpawn, ref smallRoomSpawnPoints, smallRoomPrefabs);
    }

    private void SpawnRooms(ref int roomsSpawned,int minRoomsToSpawn, int maxRoomsToSpawn, ref List<Transform> _spawnPoints, GameObject[] _prefabList)
    {
        int roomToSpawnIndex = 0;
        int roomsToSpawn = UnityEngine.Random.Range(minRoomsToSpawn, maxRoomsToSpawn);
        for (roomsSpawned = 0; roomsSpawned < roomsToSpawn; roomsSpawned++)
        {
            if (_spawnPoints.Count == 0) { Debug.LogError("Not enough spawn points to spawn rooms"); return; }

            int _randomSpawnPointIndex = UnityEngine.Random.Range(0, _spawnPoints.Count);
            roomToSpawnIndex = UnityEngine.Random.Range(0, _prefabList.Length);

            GetRoomSpawnData(_prefabList, _spawnPoints, roomToSpawnIndex, _randomSpawnPointIndex, out GameObject _roomToSpawn,
                                                out Vector3 _roomSpawnPosition, out Quaternion _roomRotation);

            GameObject _roomSpawned = Instantiate(_roomToSpawn, _roomSpawnPosition, _roomRotation);
            DestroyAndRemoveSpawnPoint(_randomSpawnPointIndex, ref _spawnPoints);
            roomToSpawnIndex++;
        }
    }

    void GetRoomSpawnData(GameObject[] _prefabList, List<Transform> _spawnPoints, int _index, int _randomSpawnPointIndex, out GameObject _roomToSpawn, out Vector3 _roomSpawnPosition, out Quaternion _roomRotation)
    {
        _roomToSpawn = _prefabList[_index];
        _roomSpawnPosition = _spawnPoints[_randomSpawnPointIndex].position;
        _roomSpawnPosition.y = _spawnPoints[_randomSpawnPointIndex].position.y;
        _roomRotation = _spawnPoints[_randomSpawnPointIndex].rotation;
    }

    private void DestroyAndRemoveSpawnPoint(int _randomSpawnPointIndex, ref List<Transform> _spawnPoints)
    {
        if (destroySpawnPointParentOnSpawn)
        {
            Destroy(_spawnPoints[_randomSpawnPointIndex].gameObject.transform.parent.gameObject);
        }
        else
        {
            Destroy(_spawnPoints[_randomSpawnPointIndex].gameObject);
        }

        _spawnPoints.RemoveAt(_randomSpawnPointIndex);
    }
}
