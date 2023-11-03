using System;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveEffect : MonoBehaviour, IProjectileEffect
{
    float explosionRadius = 5;
    float explosionForce = 700;
    LayerMask layersToIgnore;
    private GameObject explosiveEffect;
    public void Setup(float explosionRadius, float explosionForce, LayerMask layersToIgnore, GameObject explosiveEffect)
    {
        this.explosionRadius = explosionRadius;
        this.explosionForce = explosionForce;
        this.layersToIgnore = layersToIgnore;
        this.explosiveEffect = explosiveEffect;
    }

    public void ApplyEffect(IDamagable damagable, float damage, List<IProjectileEffect> projectileEffects)
    {
        projectileEffects.RemoveAll((effect) => EffectsToRemove().Contains(effect.GetType()));
        Explode(damage / 2, projectileEffects);
        GameObject instiatedParticles = Instantiate(explosiveEffect, damagable.gameObject.transform.position, damagable.gameObject.transform.rotation);
        Invoke("DeleteVFX", 1f);
    }

    public List<Type> EffectsToRemove()
    {
        return new List<Type> { typeof(ExplosiveEffect), typeof(ScatterEffect) };
    }

    void Explode(float damage, List<IProjectileEffect> projectileEffects)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius, ~layersToIgnore);
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

    void DeleteVFX(GameObject gameObject)
    {
        Destroy(gameObject);
    }
}