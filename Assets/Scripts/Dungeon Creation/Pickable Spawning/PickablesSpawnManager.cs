using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class PickablesSpawnManager : PickableBaseSpawning
{
    [Header("PrefabsToSpawn")]
    [SerializeField] AllWeaponRarities weaponRarities;
    [SerializeField] ExpOrbRarities expOrbRarities;
    [SerializeField] AllConsumableRarities consumableRarities;

    [Header("Pickables To Spawn")]
    [SerializeField] bool SpawnWeapons;
    [SerializeField] bool SpawnConsumables;
    [SerializeField] bool SpawnExpOrbs;

    [Header("Weapon Spawning Settings")]
    [SerializeField] int minimumWeaponsToSpawn;
    [SerializeField][Range(0, 100)] int CommonWeaponsProbability = 70;
    [SerializeField][Range(0, 100)] int RareWeaponsProbability = 40;
    [SerializeField][Range(0, 100)] int LegendaryWeaponsProbability = 10;

    [Header("Consumable Spawn Settings")]
    [SerializeField] int minimumConsumablesToSpawn;
    [SerializeField][Range(0, 100)] int CommonConsumablesProbability = 70;
    [SerializeField][Range(0, 100)] int RareConsumablesProbability = 40;
    [SerializeField][Range(0, 100)] int LegendaryConsumablesProbability = 10;

    [Header("Exp Orbs  Spawn Settings")]
    [SerializeField] int minimumExpToSpawn = 500;
    [SerializeField][Range(0, 100)] int LowExpOrbProbability = 70;
    [SerializeField][Range(0, 100)] int MediumExpOrbProbability = 40;
    [SerializeField][Range(0, 100)] int HighExpOrbProbability = 10;

    [Header("Spawning Debug Stats")]
    [SerializeField] private int weaponsSpawned;
    [SerializeField] private int consumablesSpawned;
    [SerializeField] private int expOrbsSpawned;
    [SerializeField] private int totalExpSpawnedInSoFar;

    [Header("SpawnPoints Info")]
    [SerializeField] Transform allSpawnPointsParent;
    public List<Transform> AllPickableSpawnPoints = new List<Transform>();



    private void Awake()
    {
        AllPickableSpawnPoints.RemoveAll(item => item == null);
    }
    private void Start()
    {
        //InitializeExpOrbTypesDictionary();
        SpawnAllWeapons();
        //SpawnAllExpOrbs();
        // SpawnAllConsumables();
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

    void SpawnAllWeapons()
    {
        if(!SpawnWeapons) { return; }
        for (weaponsSpawned = 0; weaponsSpawned < minimumWeaponsToSpawn;)
        {
            if (weaponRarities.AllWeaponTypes.Count == 0) { return; }
            int _randomWeaponRarityNumber = Random.Range(0, 100);
            bool _isAbleToSpawn = false;

            if (_randomWeaponRarityNumber <= CommonWeaponsProbability && _randomWeaponRarityNumber > RareWeaponsProbability && _randomWeaponRarityNumber > LegendaryWeaponsProbability)
            {
                ItemRarityListClass CommonWeaponRarityList = weaponRarities.AllWeaponTypes[0];
                int _randomWeaponIndex = Random.Range(0, CommonWeaponRarityList.ItemRarityList.Count);
                GameObject _weaponToSpawn = CommonWeaponRarityList.ItemRarityList[_randomWeaponIndex];
                _isAbleToSpawn = SpawnAPickableAtRandomSpawnPoint(_weaponToSpawn, AllPickableSpawnPoints);
                if (_isAbleToSpawn) { weaponsSpawned++; }
                continue;
            }

            if (_randomWeaponRarityNumber <= RareWeaponsProbability && _randomWeaponRarityNumber > LegendaryWeaponsProbability)
            {
                ItemRarityListClass RareWeaponRarityList = weaponRarities.AllWeaponTypes[1];
                int _randomWeaponIndex = Random.Range(0, RareWeaponRarityList.ItemRarityList.Count);
                GameObject _weaponToSpawn = RareWeaponRarityList.ItemRarityList[_randomWeaponIndex];
                _isAbleToSpawn = SpawnAPickableAtRandomSpawnPoint(_weaponToSpawn, AllPickableSpawnPoints);
                if (_isAbleToSpawn) { weaponsSpawned++; }
                continue;
            }

            if (_randomWeaponRarityNumber <= LegendaryWeaponsProbability)
            {
                ItemRarityListClass LegendaryWeaponRarityList = weaponRarities.AllWeaponTypes[2];
                int _randomWeaponIndex = Random.Range(0, LegendaryWeaponRarityList.ItemRarityList.Count);
                GameObject _weaponToSpawn = LegendaryWeaponRarityList.ItemRarityList[_randomWeaponIndex];
                _isAbleToSpawn = SpawnAPickableAtRandomSpawnPoint(_weaponToSpawn, AllPickableSpawnPoints);
                if (_isAbleToSpawn) { weaponsSpawned++; }
                continue;
            }
        }
    }

    bool SpawnObjectFromWeaponRarityTypesList(int _randomRarityNumber, int _targetProbability, ref int _itemsSpawned, int _rarityIndex)
    {
        if (_randomRarityNumber <= _targetProbability)
        {
            ItemRarityListClass ItemRarityList = weaponRarities.AllWeaponTypes[_rarityIndex];
            int _randomWeaponIndex = Random.Range(0, ItemRarityList.ItemRarityList.Count);
            GameObject _weaponToSpawn = ItemRarityList.ItemRarityList[_randomWeaponIndex];
            bool _isAbleToSpawn = SpawnAPickableAtRandomSpawnPoint(_weaponToSpawn, AllPickableSpawnPoints);
            if (!_isAbleToSpawn) return false;
            _itemsSpawned++; return true;     
        }
        return false;
    }




    /*  void SpawnAllConsumables()
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
      } */

    /*  void SpawnAllExpOrbs()
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
      } */

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
