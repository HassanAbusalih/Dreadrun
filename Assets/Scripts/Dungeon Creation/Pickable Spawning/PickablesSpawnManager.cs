using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PickablesSpawnManager : MonoBehaviour
{
    [Header("Weapon Spawn Settings")]
    [SerializeField] int amountOfWeaponsToSpawn;
    [SerializeField] private GameObject[] weaponPrefabs;
   
    [Header("Consumable Spawn Settings")]
    [SerializeField] int amountOfConsumablesToSpawn;
    [SerializeField] private GameObject[] consumablePrefabs;

    [Header("Exp Orbs  Spawn Settings")]
    [Tooltip("Exp amount is the actual total value of exp it has to spawn based on given spawn points")]
    [SerializeField] int amountOfExpToSpawn = 500;
    [SerializeField] private GameObject[] expOrbPrefabs;
   

    [Header("Exp Orb Value Types")]
    [SerializeField] int lowExpOrbValue = 10;
    [SerializeField] int mediumExpOrbValue = 20;
    [SerializeField] int highExpOrbValue = 30;
    Dictionary<GameObject, int> allExpOrbValueTypesDictionary = new Dictionary<GameObject, int>();


    [Header("Spawning Debug Stats")]
    [SerializeField] private int numberOfWeaponsSpawned;
    [SerializeField] private int numberOfConsumablesSpawned;
    [SerializeField] private int numberOfExpOrbsSpawned;
    [SerializeField] private int totalExpSpawnedInSoFar;

    public static List<Transform> AllPickableSpawnPoints = new List<Transform>();



    private void Start()
    {
        InitializeAllExpOrbValueTypesDictionary();
        StartTheProcessOfSpawningAllPickablesAtRandomSpawnPoints();
    }

    private void StartTheProcessOfSpawningAllPickablesAtRandomSpawnPoints()
    {
        SpawnAllExpOrbs();
    }


    void SpawnAllExpOrbs()
    {
        for (numberOfExpOrbsSpawned = 0; totalExpSpawnedInSoFar < amountOfExpToSpawn;)
        {
            int _probabilityToSpawnExpOrb = Random.Range(0, 10);
            if (_probabilityToSpawnExpOrb <= 4) { continue; } // 40% chance, that it may not spawn an exp orb

            GameObject expOrbTypeToSpawn = expOrbPrefabs[Random.Range(0, expOrbPrefabs.Length)];
            int _expOrbValueToSpawn = allExpOrbValueTypesDictionary[expOrbTypeToSpawn];

            SpawnAPickableAtRandomSpawnPoint(expOrbTypeToSpawn);
            UpdateExpOrbSpawningStats(_expOrbValueToSpawn);
        }
    }




    void SpawnAPickableAtRandomSpawnPoint(GameObject _pickableToSpawn)
    {
        int _randomPickableSpawnPointIndex = Random.Range(0, AllPickableSpawnPoints.Count);
        Vector3 _randomSpawnPosition = AllPickableSpawnPoints[_randomPickableSpawnPointIndex].position;

        GameObject _pickableSpawned = Instantiate(_pickableToSpawn, _randomSpawnPosition, Quaternion.identity);
        _pickableSpawned.transform.parent = this.transform;
        AllPickableSpawnPoints.RemoveAt(_randomPickableSpawnPointIndex);
    }

    void UpdateExpOrbSpawningStats(int _expValue)
    {
        totalExpSpawnedInSoFar += _expValue;
        numberOfExpOrbsSpawned++;
    }
    private void InitializeAllExpOrbValueTypesDictionary()
    {
        if (expOrbPrefabs.Length == 0) { return; }

        for (int i = 0; i < expOrbPrefabs.Length; i++)
        {
            allExpOrbValueTypesDictionary.Add(expOrbPrefabs[i], lowExpOrbValue);
        }
    }


}
