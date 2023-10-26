using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomEnemySpawner : PickableBaseSpawning
{
    [SerializeField] List<EnemySpawnProbability> enemySpawnProbabilities;

    [SerializeField] int enemiesToSpawn;
    [SerializeField] int enemiesSpawned;
    public static List<Transform> EnemySpawnPoints = new List<Transform>();


    private void Start()
    {
        SpawnEnemiesRepeatedly();
    }
    void SpawnEnemiesRepeatedly()
    {
        if (enemySpawnProbabilities.Count == 0) { return; }
        for (enemiesSpawned = 0; enemiesSpawned < enemiesToSpawn; )
        {
            int _randomConsumablePrefabIndex = Random.Range(0, enemySpawnProbabilities.Count);
            EnemySpawnProbability _enemyToSpawn = enemySpawnProbabilities[_randomConsumablePrefabIndex];

            float randomNumber = Random.Range(0, 100f);
            if (randomNumber > _enemyToSpawn.probabilityToSpawn) {continue; }

            bool _isAbleToSpawn = SpawnAPickableAtRandomSpawnPoint(_enemyToSpawn.enemyPrefab, EnemySpawnPoints); 
            if (!_isAbleToSpawn) return;
            enemiesSpawned++;
        }
    }

}

