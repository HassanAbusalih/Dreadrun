using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PickablesSpawnManager : MonoBehaviour
{
    [Header("Weapon Spawn Settings")]
    [SerializeField] int amountOfWeaponsToSpawn;
    [SerializeField] private List<GameObject> weaponPrefabs;

    [Header("Consumable Spawn Settings")]
    [SerializeField] int amountOfConsumablesToSpawn;
    [SerializeField] private List<GameObject> consumablePrefabs;

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
    [SerializeField] private int weaponsSpawned;
    [SerializeField] private int consumablesSpawned;
    [SerializeField] private int expOrbsSpawned;
    [SerializeField] private int totalExpSpawnedInSoFar;

    public static List<Transform> AllPickableSpawnPoints = new List<Transform>();

    private void Start()
    {
        InitializeExpOrbTypesDictionary();
        SpawnAllPickables();
    }

    private void SpawnAllPickables()
    {
        SpawnAllWeapons();
        SpawnAllConsumables();
        SpawnAllExpOrbs();
    }

    void SpawnAllWeapons()
    {
        for (weaponsSpawned = 0; weaponsSpawned < amountOfWeaponsToSpawn; weaponsSpawned++)
        {
            if (weaponPrefabs.Count == 0) { return; } // if there are no more weapons to spawn, then return
            int _randomWeaponIndex = Random.Range(0, weaponPrefabs.Count);
            GameObject weaponToSpawn = weaponPrefabs[_randomWeaponIndex];

            bool _isAbleToSpawn = SpawnAPickableAtRandomSpawnPoint(weaponToSpawn);
            if (!_isAbleToSpawn) return;

            weaponPrefabs.RemoveAt(_randomWeaponIndex);
        }
    }

    void SpawnAllConsumables()
    {
        if (consumablePrefabs.Count == 0) { return; }
        for (consumablesSpawned = 0; consumablesSpawned < amountOfConsumablesToSpawn; consumablesSpawned++)
        {
            int _probabilityToSpawnConsumable = Random.Range(0, 10);
            if (_probabilityToSpawnConsumable <= 4) { continue; } // 40% chance, that it may not spawn a consumable at a spawn point

            int _randomConsumablePrefabIndex = Random.Range(0, consumablePrefabs.Count);
            GameObject _consumableToSpawn = consumablePrefabs[_randomConsumablePrefabIndex];

            bool _isAbleToSpawn = SpawnAPickableAtRandomSpawnPoint(_consumableToSpawn);
            if (!_isAbleToSpawn) return;

        }
    }

    void SpawnAllExpOrbs()
    {
        if (expOrbPrefabs.Length == 0) { return; }
        for (expOrbsSpawned = 0; totalExpSpawnedInSoFar < amountOfExpToSpawn; expOrbsSpawned++)
        {
            int _probabilityToSpawnExpOrb = Random.Range(0, 10);
            if (_probabilityToSpawnExpOrb <= 4) { continue; } // 40% chance, that it may not spawn an exp orb

            GameObject expOrbTypeToSpawn = expOrbPrefabs[Random.Range(0, expOrbPrefabs.Length)];
            int _expValue = allExpOrbValueTypesDictionary[expOrbTypeToSpawn];

            bool _isAbleToSpawn = SpawnAPickableAtRandomSpawnPoint(expOrbTypeToSpawn);
            if (!_isAbleToSpawn) return;
            totalExpSpawnedInSoFar += _expValue;
        }
    }

    bool SpawnAPickableAtRandomSpawnPoint(GameObject _pickableToSpawn)
    {
        // if there are no spawn points left, then return (this is to avoid index out of range error)
        if (AllPickableSpawnPoints.Count == 0)
        { Debug.LogWarning("There is no more pickable spawn points,so cannot spawn more, try changing spawn settings "); return false; }

        int _randomPickableSpawnPointIndex = Random.Range(0, AllPickableSpawnPoints.Count);
        Vector3 _randomSpawnPosition = AllPickableSpawnPoints[_randomPickableSpawnPointIndex].position;

        GameObject _pickableSpawned = Instantiate(_pickableToSpawn, _randomSpawnPosition, Quaternion.identity);
        _pickableSpawned.transform.parent = this.transform;
        AllPickableSpawnPoints.RemoveAt(_randomPickableSpawnPointIndex);
        return true;
    }

    private void InitializeExpOrbTypesDictionary()
    {
        if (expOrbPrefabs.Length == 0) { return; }

        allExpOrbValueTypesDictionary.Add(expOrbPrefabs[0], lowExpOrbValue);
        allExpOrbValueTypesDictionary.Add(expOrbPrefabs[1], mediumExpOrbValue);
        allExpOrbValueTypesDictionary.Add(expOrbPrefabs[2], highExpOrbValue);
    }
}