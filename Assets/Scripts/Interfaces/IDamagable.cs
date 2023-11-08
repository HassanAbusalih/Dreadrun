using UnityEngine;

public interface IDamagable 
{
    public void TakeDamage(float Amount);
    public GameObject gameObject { get; }
}