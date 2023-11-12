using System;
using UnityEngine;

public interface IDamagable 
{
    public static Action<GameObject> onDamageTaken;
    public void TakeDamage(float Amount);
    public GameObject gameObject { get; }
}