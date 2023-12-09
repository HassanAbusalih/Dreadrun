using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    [SerializeField] int maxEnemiesTotal = 500;
    [SerializeField] int maxEnemiesPerType = 100;
    public static EnemyPool Instance { get; private set; }
    public List<EnemyAIBase> Enemies { get; private set; } = new();
    public int MaxEnemiesTotal { get => maxEnemiesTotal; }
    public int MaxEnemiesPerType { get => maxEnemiesPerType; }
    public Dictionary<Type, List<EnemyAIBase>> EnemiesBySubtype { get; private set; } = new Dictionary<Type, List<EnemyAIBase>>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            foreach (var enemy in FindObjectsOfType<EnemyAIBase>())
            {
                Add(enemy);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Add(EnemyAIBase enemy)
    {
        Enemies.Add(enemy);
        Type enemyType = enemy.GetType();
        if (!EnemiesBySubtype.ContainsKey(enemyType))
        {
            EnemiesBySubtype[enemyType] = new List<EnemyAIBase>();
        }
        EnemiesBySubtype[enemyType].Add(enemy);
    }

    public void Remove(EnemyAIBase enemy)
    {
        Enemies.Remove(enemy);
        Type enemyType = enemy.GetType();
        if (EnemiesBySubtype.ContainsKey(enemyType))
        {
            EnemiesBySubtype[enemyType].Remove(enemy);
        }
    }
}