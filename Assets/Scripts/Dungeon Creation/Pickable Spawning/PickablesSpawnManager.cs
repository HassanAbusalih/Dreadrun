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
    [SerializeField] int TotalLootPoints;
    [SerializeField] private int weaponsSpawned;
    [SerializeField] private int consumablesSpawned;
    [SerializeField] private int expOrbsSpawned;
    [SerializeField] private int totalExpSpawnedInSoFar;

    [Header("SpawnPoints Info")]
    public Transform SpawnPointsParent;
    public List<Transform> PickableSpawnPoints = new List<Transform>();

    public Action<int> OnAllItemsSpawned;

    private void Start()
    {
        SpawnAllWeapons();
        SpawnAllExpOrbs();
        SpawnAllConsumables();
        OnAllItemsSpawned?.Invoke(TotalLootPoints);
    }

    void SpawnAllWeapons()
    {
        if (!SpawnWeapons) { return; }
        for (weaponsSpawned = 0; weaponsSpawned < minimumWeaponsToSpawn;)
        {
            if (weaponRarities.AllWeaponTypes.Count == 0) { return; }
            if(PickableSpawnPoints.Count == 0) { return; }
            int _randomWeaponRarityNumber = Random.Range(0, 100);

            if (SpawnItemFromRarityTypes(weaponRarities.AllWeaponTypes, _randomWeaponRarityNumber,
                LegendaryWeaponsProbability, ref weaponsSpawned, 2)) continue;
            if (SpawnItemFromRarityTypes(weaponRarities.AllWeaponTypes, _randomWeaponRarityNumber,
                RareWeaponsProbability, ref weaponsSpawned, 1)) continue;
            SpawnItemFromRarityTypes(weaponRarities.AllWeaponTypes, _randomWeaponRarityNumber,
                CommonWeaponsProbability, ref weaponsSpawned, 0);
        }
    }

    void SpawnAllConsumables()
    {
        if (!SpawnConsumables) { return; }
        for (consumablesSpawned = 0; consumablesSpawned < minimumConsumablesToSpawn;)
        {
            if (consumableRarities.AllConsumableTypes.Count == 0) { return; }
            if (PickableSpawnPoints.Count == 0) { return; }
            int _randomConsumableRarityNumber = Random.Range(0, 100);

            if (SpawnItemFromRarityTypes(consumableRarities.AllConsumableTypes, _randomConsumableRarityNumber,
                LegendaryConsumablesProbability, ref consumablesSpawned, 2)) continue;
            if (SpawnItemFromRarityTypes(consumableRarities.AllConsumableTypes, _randomConsumableRarityNumber,
                RareConsumablesProbability, ref consumablesSpawned, 1)) continue;
            SpawnItemFromRarityTypes(consumableRarities.AllConsumableTypes, _randomConsumableRarityNumber,
                CommonConsumablesProbability, ref consumablesSpawned, 0);
        }
    }

    void SpawnAllExpOrbs()
    {
        if (!SpawnExpOrbs) { return; }
        for (expOrbsSpawned = 0; totalExpSpawnedInSoFar < minimumExpToSpawn;)
        {
            if (expOrbRarities.AllExpOrbTypes.Count == 0) { return; }
            if (PickableSpawnPoints.Count == 0) { return; }
            int _randomExpOrbRarityNumber = Random.Range(0, 100);

            if (SpawnExpOrbBasedOnProbability(expOrbRarities.AllExpOrbTypes, _randomExpOrbRarityNumber,
                HighExpOrbProbability, ref expOrbsSpawned, 2)) continue;
            if (SpawnExpOrbBasedOnProbability(expOrbRarities.AllExpOrbTypes, _randomExpOrbRarityNumber,
                MediumExpOrbProbability, ref expOrbsSpawned, 1)) continue;
            SpawnExpOrbBasedOnProbability(expOrbRarities.AllExpOrbTypes, _randomExpOrbRarityNumber,
                LowExpOrbProbability, ref expOrbsSpawned, 0);
        }
    }
    #region Item Spawning Helper Functions

    protected bool SpawnItemFromRarityList(ref int _itemsSpawned, ItemRarityListClass ItemRarityList)
    {
        int _randomItemIndex = Random.Range(0, ItemRarityList.ItemRarityList.Count);
        GameObject _itemToSpawn = ItemRarityList.ItemRarityList[_randomItemIndex];
        bool _isAbleToSpawn = SpawnAPickableAtRandomSpawnPoint(_itemToSpawn, PickableSpawnPoints);
        if (!_isAbleToSpawn) return false;
        TotalLootPoints += (int)ItemRarityList.ListRarityType;
        _itemsSpawned++; return true;
    }

    // i screwed up in my structure initially and thats why this function exists
    protected bool SpawnExpOrbFromRarityList(ref int _itemsSpawned, List<ItemRarityClass> _ExpOrbRarityList, int _prefabIndex)
    {
        GameObject _expOrbToSpawn = _ExpOrbRarityList[_prefabIndex].Item;
        bool _isAbleToSpawn = SpawnAPickableAtRandomSpawnPoint(_expOrbToSpawn, PickableSpawnPoints);
        if (!_isAbleToSpawn) return false;
        totalExpSpawnedInSoFar += _expOrbToSpawn.GetComponent<ExpOrb>().ExpAmount;
        TotalLootPoints += (int)_ExpOrbRarityList[_prefabIndex].RarityType;
        _itemsSpawned++; return true;
    }

    bool SpawnItemFromRarityTypes(List<ItemRarityListClass> _itemRarityTypes, int _randomRarityNumber, int _targetProbability, ref int _itemsSpawned, int _rarityIndex)
    {
        if (_randomRarityNumber <= _targetProbability)
        {
            ItemRarityListClass ItemRarityList = _itemRarityTypes[_rarityIndex];
            return SpawnItemFromRarityList(ref _itemsSpawned, ItemRarityList);
        }
        return false;
    }
    // i screwed up in my structure initially and thats why this function exists
    bool SpawnExpOrbBasedOnProbability(List<ItemRarityClass> _itemRarityList, int _randomRarityNumber, int _targetProbability, ref int _itemsSpawned, int _prefabRarityIndex)
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
        ShowErrorWhenCantSpawnAllItems();
        AddChildrenToSpawnPointsList(SpawnPointsParent, PickableSpawnPoints);
    }

    void ShowErrorWhenCantSpawnAllItems()
    {
        float _totalItemsToSpawn = minimumWeaponsToSpawn + minimumConsumablesToSpawn + (minimumExpToSpawn / 20);
        float _totalSpawnPoints = PickableSpawnPoints.Count;
        if (_totalItemsToSpawn > _totalSpawnPoints)
        {
            //Debug.LogError("There are not enough spawn points to spawn all the items, try reducing the minimum amounts of certain items");
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
            _pickablesSpawnManager.AddChildrenToSpawnPointsList(_pickablesSpawnManager.SpawnPointsParent, _pickablesSpawnManager.PickableSpawnPoints);
        }
    }
}
#endif
