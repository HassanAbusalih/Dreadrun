using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RandomEnemySpawner : PickableBaseSpawning
{
    [SerializeField] List<EnemySpawnProbability> enemySpawnProbabilities;

    [Header("Spawn Based On LootQualify Settings")]
    [SerializeField] PickablesSpawnManager pickablesSpawnManager;
    [SerializeField] bool spawnEnemiesNormally;

    [Header("Loot QualityPoints Limit Settings")]
    [SerializeField] int badLootPointsLimit;
    [SerializeField] int mediumLootPointsLimit;
    [SerializeField] int goodLootPointsLimit;

    [Header("Enemies To Spawn Settings")]
    [SerializeField] int minAmountOfEnemiesToSpawn;
    [SerializeField] int midAmountOfEnemiesToSpawn;
    [SerializeField] int maxAmountOfEnemiesToSpawn;

    [Header("Debug Info")]
    [SerializeField] int enemiesSpawned;
    [SerializeField] int SpawnedInLootPoints;

    [Header("SpawnPoints Info")]
    public Transform SpawnPointsParent;
    public List<Transform> EnemySpawnPoints = new List<Transform>();


    private void Start()
    {
        if (spawnEnemiesNormally) { SpawnEnemies(minAmountOfEnemiesToSpawn); }
    }

    void SpawnEnemiesBasedOnLootPoints(int _totalLootPoints)
    {
        SpawnedInLootPoints = _totalLootPoints;
        if (_totalLootPoints <= badLootPointsLimit) { SpawnEnemies(minAmountOfEnemiesToSpawn); }
        else if (_totalLootPoints <= mediumLootPointsLimit) { SpawnEnemies(midAmountOfEnemiesToSpawn); }
        else if (_totalLootPoints <= goodLootPointsLimit || _totalLootPoints > goodLootPointsLimit) { SpawnEnemies(maxAmountOfEnemiesToSpawn); }
    }

    void SpawnEnemies(int amountToSpawn)
    {
        if (enemySpawnProbabilities.Count == 0) { return; }
        for (enemiesSpawned = 0; enemiesSpawned < amountToSpawn;)
        {
            int _randomEnemyPrefabIndex = Random.Range(0, enemySpawnProbabilities.Count);
            EnemySpawnProbability _enemyToSpawn = enemySpawnProbabilities[_randomEnemyPrefabIndex];

            float randomNumber = Random.Range(0, 100f);
            if (randomNumber > _enemyToSpawn.probabilityToSpawn) { continue; }

            bool _isAbleToSpawn = SpawnAPickableAtRandomSpawnPoint(_enemyToSpawn.enemyPrefab, EnemySpawnPoints);
            if (!_isAbleToSpawn) return;
            enemiesSpawned++;
        }
    }

    private void OnValidate()
    {
        ShowErrorWhenCantSpawnAllEnemies();
        AddChildrenToSpawnPointsList(SpawnPointsParent,EnemySpawnPoints);
        if(!spawnEnemiesNormally)
        pickablesSpawnManager = FindObjectOfType<PickablesSpawnManager>();
    }

    private void OnEnable()
    {
        if (pickablesSpawnManager == null) return;
        pickablesSpawnManager.OnAllItemsSpawned += SpawnEnemiesBasedOnLootPoints;
    }

    private void OnDisable()
    {
        if (pickablesSpawnManager == null) return;
        pickablesSpawnManager.OnAllItemsSpawned -= SpawnEnemiesBasedOnLootPoints;
    }

    void ShowErrorWhenCantSpawnAllEnemies()
    {
        float _totalPossibleEnemiesToSpawn = maxAmountOfEnemiesToSpawn;
        float _totalSpawnPoints = EnemySpawnPoints.Count;
        if (_totalPossibleEnemiesToSpawn < _totalSpawnPoints) return;
        Debug.LogError("There are not enough spawn points to spawn all the ENEMIES, try reducing the minimum amounts of certain items");
    }
}
#if UNITY_EDITOR
[CustomEditor(typeof(RandomEnemySpawner))]
public class RandomEnemySpawnerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        RandomEnemySpawner _RandomEnemySpawner = (RandomEnemySpawner)target;

        if (GUILayout.Button("UpdateSpawnPointsList"))
        {
            _RandomEnemySpawner.AddChildrenToSpawnPointsList(_RandomEnemySpawner.SpawnPointsParent, _RandomEnemySpawner.EnemySpawnPoints);
        }
    }
}
#endif


