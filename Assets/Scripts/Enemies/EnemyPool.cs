using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    public static EnemyPool Instance { get; private set; }

    public List<EnemyAIBase> Enemies { get; private set; } = new();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Add(EnemyAIBase enemy)
    {
        Enemies.Add(enemy);
    }

    public void Remove(EnemyAIBase enemy)
    {
        Enemies.Remove(enemy);
    }
}