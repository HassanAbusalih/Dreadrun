using System;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveEffect : MonoBehaviour, IProjectileEffect
{
    [SerializeField] float explosionRadius = 5;
    [SerializeField] float explosionForce = 700;

    LayerMask mask = ((1 << 8) | (1 << 9));
    public void ApplyEffect(IDamagable damagable, float damage, List<IProjectileEffect> projectileEffects)
    {
        projectileEffects.RemoveAll((effect) => EffectsToRemove().Contains(effect.GetType()));
        Explode(damage / 2, projectileEffects);
    }

    public List<Type> EffectsToRemove()
    {
        return new List<Type> { typeof(ExplosiveEffect), typeof(ScatterEffect) };
    }

    void Explode(float damage, List<IProjectileEffect> projectileEffects)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius, mask);
        foreach (Collider nearbyObject in colliders)
        {
            if (nearbyObject.TryGetComponent(out Rigidbody rb))
            {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius, 0, ForceMode.Impulse);
            }
            if (nearbyObject.TryGetComponent(out IDamagable newDamagable))
            {
                newDamagable.TakeDamage(damage);
                foreach (IProjectileEffect effect in projectileEffects)
                {
                    effect.ApplyEffect(newDamagable, damage, new List<IProjectileEffect>(projectileEffects));
                }
            }
        }
    }
}