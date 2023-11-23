using System;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveEffect : MonoBehaviour, IProjectileEffect
{
    float explosionRadius = 5;
    float explosionForce = 700;
    LayerMask layersToIgnore;
    private GameObject explosiveEffect;

    public bool ApplyOnCollision { get; } = true;

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
        Explosion.Explode(damagable.gameObject.transform, damage / 2, explosionRadius, explosionForce, layersToIgnore, projectileEffects);
        GameObject instantiatedParticles = Instantiate(explosiveEffect, damagable.gameObject.transform.position, damagable.gameObject.transform.rotation);
    }

    public void ApplyEffect(Transform point, float damage, List<IProjectileEffect> projectileEffects)
    {
        projectileEffects.RemoveAll((effect) => EffectsToRemove().Contains(effect.GetType()));
        Explosion.Explode(point.position, damage / 2, explosionRadius, explosionForce, layersToIgnore, projectileEffects);
        GameObject instantiatedParticles = Instantiate(explosiveEffect, point.position, point.rotation);
    }

    public List<Type> EffectsToRemove()
    {
        return new List<Type> { typeof(ExplosiveEffect), typeof(ScatterEffect) };
    }
}