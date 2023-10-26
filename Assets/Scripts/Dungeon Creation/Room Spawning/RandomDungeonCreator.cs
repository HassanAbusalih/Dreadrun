using System;
using System.Collections.Generic;
using UnityEngine;


public class RandomDungeonCreator : MonoBehaviour
{

    public static List<Transform> smallRoomSpawnPoints = new List<Transform>();
    [Header("Small Rooms Spawn Settings")]
    [SerializeField] int smallRoomsToSpawn;
    [SerializeField] int smallRoomsToSkipAfterFirstIteration;
    [SerializeField] GameObject[] smallRoomPrefabs;

    public static List<Transform> bigRoomSpawnPoints = new List<Transform>();
    [Header("Big Rooms Spawn Settings")]
    [SerializeField] int bigRoomsToSpawn;
    [SerializeField] int bigRoomsToSkipAfterFirstIteration;
    [SerializeField] GameObject[] bigRoomPrefabs;

    [Header("Debug Stats")]
    [SerializeField] int smallRoomsSpawned;
    [SerializeField] int bigRoomsSpawned;

    [SerializeField] bool destroySpawnPointParentOnSpawn;




    private void Start()
    {
        bigRoomSpawnPoints.RemoveAll(item => item == null);
        smallRoomSpawnPoints.RemoveAll(item => item == null);

        StartRandomRoomSpawningProcess();
    }

    void StartRandomRoomSpawningProcess()
    {
        SpawnRooms(ref bigRoomsSpawned, bigRoomsToSpawn, ref bigRoomSpawnPoints, bigRoomPrefabs, bigRoomsToSkipAfterFirstIteration);
        SpawnRooms(ref smallRoomsSpawned, smallRoomsToSpawn, ref smallRoomSpawnPoints, smallRoomPrefabs, smallRoomsToSkipAfterFirstIteration);
    }

    private void SpawnRooms(ref int roomsSpawned, int _roomsToSpawn, ref List<Transform> _spawnPoints, GameObject[] _prefabList, int roomsToSkip)
    {
        int roomToSpawnIndex = 0;
        for (roomsSpawned = 0; roomsSpawned < _roomsToSpawn; roomsSpawned++)
        {
            if (_spawnPoints.Count == 0) { Debug.LogError("Not enough spawn points to spawn rooms"); return; }

            int _randomSpawnPointIndex = UnityEngine.Random.Range(0, _spawnPoints.Count);
            ResetRoomPrefabsToSpawnIndex(ref roomToSpawnIndex, roomsToSkip, _prefabList);

            GetRoomSpawnData(_prefabList, _spawnPoints, roomToSpawnIndex, _randomSpawnPointIndex, out GameObject _roomToSpawn,
                                                out Vector3 _roomSpawnPosition, out Quaternion _roomRotation);

            GameObject _roomSpawned = Instantiate(_roomToSpawn, _roomSpawnPosition, _roomRotation);
            DestroyAndRemoveSpawnPoint(_randomSpawnPointIndex, ref _spawnPoints);
            roomToSpawnIndex++;
        }
    }

    int ResetRoomPrefabsToSpawnIndex(ref int _currentRoomToSpawnIndex, int roomsToSkip, GameObject[] roomPrefabList)
    {
        if (_currentRoomToSpawnIndex == roomPrefabList.Length)
        {
            _currentRoomToSpawnIndex = 0 + roomsToSkip;
            return _currentRoomToSpawnIndex;
        }
        else { return _currentRoomToSpawnIndex; }
    }

    void GetRoomSpawnData(GameObject[] _prefabList, List<Transform> _spawnPoints, int _index, int _randomSpawnPointIndex, out GameObject _roomToSpawn, out Vector3 _roomSpawnPosition, out Quaternion _roomRotation)
    {
        _roomToSpawn = _prefabList[_index];
        _roomSpawnPosition = _spawnPoints[_randomSpawnPointIndex].position;
        _roomSpawnPosition.y = _spawnPoints[_randomSpawnPointIndex].root.position.y;
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
