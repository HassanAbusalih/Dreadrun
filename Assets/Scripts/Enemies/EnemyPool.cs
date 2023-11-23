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
            foreach(var enemy in FindObjectsOfType<EnemyAIBase>())
            {
                if(!Enemies.Contains(enemy))
                {
                    Enemies.Add(enemy);
                }
            }
        }
        else
        {
            Destroy(this);
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