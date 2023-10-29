using System;
using UnityEngine;
using UnityEngine.Events;

public class OnHitDamagable : MonoBehaviour, IDamagable
{
    [SerializeField] private bool destroyOnDamage;
    [SerializeField] protected private UnityEvent onTakeDamage;
    

    public void TakeDamage(float damage)
    {
        onTakeDamage?.Invoke();
        DoSomethingWhenObjectIsHit();
        DestroyObject();
    }

    private void DestroyObject()
    {
        if (destroyOnDamage)
        {
            Destroy(gameObject);
        }
       
    }
    protected virtual void DoSomethingWhenObjectIsHit()
    {

    }
}

