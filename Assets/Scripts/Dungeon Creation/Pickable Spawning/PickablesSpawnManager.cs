using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class PickablesSpawnManager : PickableBaseSpawning
{
    [Header("Weapon Spawn Settings")]
    [SerializeField] int minimumWeaponsToSpawn;
    [SerializeField] private List<GameObject> weaponPrefabs;

    [Header("Consumable Spawn Settings")]
    [SerializeField] int minimumConsumablesToSpawn;
    [SerializeField] private List<GameObject> consumablePrefabs;

    [Header("Exp Orbs  Spawn Settings")]
    [Tooltip("Exp amount is the actual total value of exp it has to spawn based on given spawn points")]
    [SerializeField] int minimumExpToSpawn = 500;
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

    [SerializeField] Transform allSpawnPointsParent;
    public List<Transform> AllPickableSpawnPoints = new List<Transform>();

    

    private void Awake()
    {
        AllPickableSpawnPoints.RemoveAll(item => item == null);
    }
    private void Start()
    {
        InitializeSpawning();
    }


    private void OnValidate()
    {
        ShowErrorWhenCantSpawnAllItems();
        AddChildrenToSpawnPointsList();  
    }

    public void AddChildrenToSpawnPointsList()
    {
        if (allSpawnPointsParent == null) { return; }
        foreach (Transform child in allSpawnPointsParent)
        {
            if (AllPickableSpawnPoints.Contains(child)) { continue; }
            AllPickableSpawnPoints.Add(child);
        }
        AllPickableSpawnPoints.RemoveAll(item => item == null);
    }

    void ShowErrorWhenCantSpawnAllItems()
    {
        float _totalItemsToSpawn = minimumWeaponsToSpawn + minimumConsumablesToSpawn + minimumExpToSpawn;
        float _totalSpawnPoints = AllPickableSpawnPoints.Count;
        if (_totalItemsToSpawn > _totalSpawnPoints)
        {
            Debug.LogError("There are not enough spawn points to spawn all the items, try reducing the minimum amounts of certain items");
        }
    }

    private void InitializeExpOrbTypesDictionary()
    {
        if (expOrbPrefabs.Length == 0) { return; }

        allExpOrbValueTypesDictionary.Add(expOrbPrefabs[0], lowExpOrbValue);
        allExpOrbValueTypesDictionary.Add(expOrbPrefabs[1], mediumExpOrbValue);
        allExpOrbValueTypesDictionary.Add(expOrbPrefabs[2], highExpOrbValue);
    }

    void InitializeSpawning()
    {
        InitializeExpOrbTypesDictionary();
        SpawnAllWeapons();
        SpawnAllExpOrbs();
        SpawnAllConsumables();
        
    }
    void SpawnAllWeapons()
    {
        for (weaponsSpawned = 0; weaponsSpawned < minimumWeaponsToSpawn; weaponsSpawned++)
        {
            if (weaponPrefabs.Count == 0) { return; } // if there are no more weapons to spawn, then return
            int _randomWeaponIndex = Random.Range(0, weaponPrefabs.Count);
            GameObject weaponToSpawn = weaponPrefabs[_randomWeaponIndex];

            bool _isAbleToSpawn = SpawnAPickableAtRandomSpawnPoint(weaponToSpawn,AllPickableSpawnPoints);
            if (!_isAbleToSpawn) return;
        }
    }

    void SpawnAllConsumables()
    {
        if (consumablePrefabs.Count == 0) { return; }
        for (consumablesSpawned = 0; consumablesSpawned < minimumConsumablesToSpawn; consumablesSpawned++)
        {
            int _probabilityToSpawnConsumable = Random.Range(0, 10);
            if (_probabilityToSpawnConsumable <= 4) { continue; } // 40% chance, that it may not spawn a consumable at a spawn point

            int _randomConsumablePrefabIndex = Random.Range(0, consumablePrefabs.Count);
            GameObject _consumableToSpawn = consumablePrefabs[_randomConsumablePrefabIndex];

            bool _isAbleToSpawn = SpawnAPickableAtRandomSpawnPoint(_consumableToSpawn, AllPickableSpawnPoints);
            if (!_isAbleToSpawn) return;
        }
    }

    void SpawnAllExpOrbs()
    {
        if (expOrbPrefabs.Length == 0) { return; }
        for (expOrbsSpawned = 0; totalExpSpawnedInSoFar < minimumExpToSpawn; expOrbsSpawned++)
        {
            int _probabilityToSpawnExpOrb = Random.Range(0, 10);
            if (_probabilityToSpawnExpOrb <= 4) { continue; } // 40% chance, that it may not spawn an exp orb

            GameObject expOrbTypeToSpawn = expOrbPrefabs[Random.Range(0, expOrbPrefabs.Length)];
            int _expValue = allExpOrbValueTypesDictionary[expOrbTypeToSpawn];

            bool _isAbleToSpawn = SpawnAPickableAtRandomSpawnPoint(expOrbTypeToSpawn, AllPickableSpawnPoints);
            if (!_isAbleToSpawn) return;
            totalExpSpawnedInSoFar += _expValue;
        }
    }

}

#if UNITY_EDITOR
[CustomEditor(typeof(PickablesSpawnManager))]
public class PickableSpawningEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        PickablesSpawnManager _pickablesSpawnManager = (PickablesSpawnManager)target;

        if (GUILayout.Button("UpdateSpawnPointsList"))
        {
            _pickablesSpawnManager.AddChildrenToSpawnPointsList();
        }     
    }
}
#endif
