using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class EnemySpawnProbability
{
    public GameObject enemyPrefab;
    [Range(0,100)]public float probabilityToSpawn;

}
