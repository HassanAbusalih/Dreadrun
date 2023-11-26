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
    [SerializeField][Range(0, 100)] float CommonWeaponsProbability = 70;
    [SerializeField][Range(0, 100)] float RareWeaponsProbability = 40;
    [SerializeField][Range(0, 100)] float LegendaryWeaponsProbability = 10;

    [Header("Consumable Spawn Settings")]
    [SerializeField] int minimumConsumablesToSpawn;
    [SerializeField][Range(0, 100)] float CommonConsumablesProbability = 70;
    [SerializeField][Range(0, 100)] float RareConsumablesProbability = 40;
    [SerializeField][Range(0, 100)] float LegendaryConsumablesProbability = 10;

    [Header("Exp Orbs  Spawn Settings")]
    [SerializeField] int minimumExpToSpawn = 500;
    [SerializeField][Range(0, 100)] float LowExpOrbProbability = 70;
    [SerializeField][Range(0, 100)] float MediumExpOrbProbability = 40;
    [SerializeField][Range(0, 100)] float HighExpOrbProbability = 10;

    [Header("Spawning Debug Stats")]
    [SerializeField] int TotalLootPoints;
    [SerializeField] private int weaponsSpawned;
    [SerializeField] private int consumablesSpawned;
    [SerializeField] private int expOrbsSpawned;
    [SerializeField] private int totalExpSpawnedInSoFar;

    [Header("SpawnPoints Info")]
    public Transform SpawnPointsParent;
    public List<Transform> PickableSpawnPoints = new List<Transform>();

    public Action<int> OnAllItemsSpawned;
    bool triggerEnemySpawnOnce;

    private void Update()
    {
        SpawnAllWeapons();
        SpawnAllExpOrbs();
        SpawnAllConsumables();

        if(!SpawnWeapons && !SpawnConsumables && !SpawnExpOrbs || PickableSpawnPoints.Count ==0)
        {
            if(triggerEnemySpawnOnce) { return; }
            OnAllItemsSpawned?.Invoke(TotalLootPoints);
            triggerEnemySpawnOnce = true;
        }
    }

    void SpawnAllWeapons()
    {
        if (!SpawnWeapons) { return; }
        if( weaponsSpawned < minimumWeaponsToSpawn)
        {
            if (weaponRarities.AllWeaponTypes.Count == 0) { return; }
            if(PickableSpawnPoints.Count == 0) { return; }
            int _randomWeaponRarityNumber = Random.Range(0, 100);

            if (SpawnItemFromRarityTypes(weaponRarities.AllWeaponTypes, _randomWeaponRarityNumber,
                LegendaryWeaponsProbability, ref weaponsSpawned, 2)) return;
            if (SpawnItemFromRarityTypes(weaponRarities.AllWeaponTypes, _randomWeaponRarityNumber,
                RareWeaponsProbability, ref weaponsSpawned, 1)) return;
            SpawnItemFromRarityTypes(weaponRarities.AllWeaponTypes, _randomWeaponRarityNumber,
                CommonWeaponsProbability, ref weaponsSpawned, 0);
            return;
        }
        SpawnWeapons = false;
    }

    void SpawnAllConsumables()
    {
        if (!SpawnConsumables) { return; }
        if (consumablesSpawned < minimumConsumablesToSpawn)
        {
            if (consumableRarities.AllConsumableTypes.Count == 0) { return; }
            if (PickableSpawnPoints.Count == 0) { return; }
            int _randomConsumableRarityNumber = Random.Range(0, 100);

            if (SpawnItemFromRarityTypes(consumableRarities.AllConsumableTypes, _randomConsumableRarityNumber,
                LegendaryConsumablesProbability, ref consumablesSpawned, 2)) return;
            if (SpawnItemFromRarityTypes(consumableRarities.AllConsumableTypes, _randomConsumableRarityNumber,
                RareConsumablesProbability, ref consumablesSpawned, 1)) return;
            SpawnItemFromRarityTypes(consumableRarities.AllConsumableTypes, _randomConsumableRarityNumber,
                CommonConsumablesProbability, ref consumablesSpawned, 0);
            return;
        }
        SpawnConsumables = false;
    }

    void SpawnAllExpOrbs()
    {
        if (!SpawnExpOrbs) { return; }
        if( totalExpSpawnedInSoFar < minimumExpToSpawn)
        {
            if (expOrbRarities.AllExpOrbTypes.Count == 0) { return; }
            if (PickableSpawnPoints.Count == 0) { return; }
            int _randomExpOrbRarityNumber = Random.Range(0, 100);

            if (SpawnExpOrbBasedOnProbability(expOrbRarities.AllExpOrbTypes, _randomExpOrbRarityNumber,
                HighExpOrbProbability, ref expOrbsSpawned, 2)) return;
            if (SpawnExpOrbBasedOnProbability(expOrbRarities.AllExpOrbTypes, _randomExpOrbRarityNumber,
                MediumExpOrbProbability, ref expOrbsSpawned, 1)) return;
            SpawnExpOrbBasedOnProbability(expOrbRarities.AllExpOrbTypes, _randomExpOrbRarityNumber,
                LowExpOrbProbability, ref expOrbsSpawned, 0);
            return;
        }
        SpawnExpOrbs = false;
    }
    #region Item Spawning Helper Functions

    protected bool SpawnItemFromRarityList(ref int _itemsSpawned, ItemRarityListClass ItemRarityList)
    {
        int _randomItemIndex = Random.Range(0, ItemRarityList.ItemRarityList.Count);
        GameObject _itemToSpawn = ItemRarityList.ItemRarityList[_randomItemIndex];
        bool _isAbleToSpawn = SpawnObjectAtRandomSpawnPoint(_itemToSpawn, PickableSpawnPoints);
        if (!_isAbleToSpawn) return false;
        TotalLootPoints += (int)ItemRarityList.ListRarityType;
        _itemsSpawned++; return true;
    }

    // i screwed up in my structure initially and thats why this function exists
    protected bool SpawnExpOrbFromRarityList(ref int _itemsSpawned, List<ItemRarityClass> _ExpOrbRarityList, int _prefabIndex)
    {
        GameObject _expOrbToSpawn = _ExpOrbRarityList[_prefabIndex].Item;
        bool _isAbleToSpawn = SpawnObjectAtRandomSpawnPoint(_expOrbToSpawn, PickableSpawnPoints);
        if (!_isAbleToSpawn) return false;
        totalExpSpawnedInSoFar += _expOrbToSpawn.GetComponent<ExpOrb>().ExpAmount;
        TotalLootPoints += (int)_ExpOrbRarityList[_prefabIndex].RarityType;
        _itemsSpawned++; return true;
    }

    bool SpawnItemFromRarityTypes(List<ItemRarityListClass> _itemRarityTypes, int _randomRarityNumber, float _targetProbability, ref int _itemsSpawned, int _rarityIndex)
    {
        if (_randomRarityNumber <= _targetProbability)
        {
            ItemRarityListClass ItemRarityList = _itemRarityTypes[_rarityIndex];
            return SpawnItemFromRarityList(ref _itemsSpawned, ItemRarityList);
        }
        return false;
    }
    // i screwed up in my structure initially and thats why this function exists
    bool SpawnExpOrbBasedOnProbability(List<ItemRarityClass> _itemRarityList, int _randomRarityNumber, float _targetProbability, ref int _itemsSpawned, int _prefabRarityIndex)
    {
        if (_randomRarityNumber <= _targetProbability)
        {
            return SpawnExpOrbFromRarityList(ref _itemsSpawned, _itemRarityList, _prefabRarityIndex);
        }
        return false;
    }
    #endregion

    private void OnValidate()
    {
        AddChildrenToSpawnPointsList(SpawnPointsParent, PickableSpawnPoints);
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
            _pickablesSpawnManager.AddChildrenToSpawnPointsList(_pickablesSpawnManager.SpawnPointsParent, _pickablesSpawnManager.PickableSpawnPoints);
        }
    }
}
#endif
