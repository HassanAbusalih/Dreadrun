using System;
using System.Collections.Generic;
using UnityEngine;

public class ScatterEffect : MonoBehaviour, IProjectileEffect
{
    int projectileCount = 4;
    float projectileSpeed = 5f;
    float projectileRange = 5f; 
    GameObject projectilePrefab;

    public void Setup(GameObject projectilePrefab, int projectileCount, float projectileSpeed, float projectileRange)
    {
        this.projectilePrefab = projectilePrefab;
        this.projectileCount = projectileCount;
        this.projectileSpeed = projectileSpeed;
        this.projectileRange = projectileRange;
    }

    public void ApplyEffect(IDamagable damagable, float damage, List<IProjectileEffect> projectileEffects)
    {
        projectileEffects.RemoveAll((effect) => EffectsToRemove().Contains(effect.GetType()));
        Scatter(damagable.gameObject.transform, damage, projectileEffects);
    }

    public List<Type> EffectsToRemove()
    {
        return new List<Type> { typeof(ScatterEffect) };
    }

    void Scatter(Transform enemy, float damage, List<IProjectileEffect> effects)
    {
        float rotationAmount = 360 / projectileCount;
        for (int i = 0; i < projectileCount; i++)
        {
            float rotation = i * rotationAmount;
            Quaternion projectileRotation = Quaternion.Euler(0, rotation, 0) * transform.rotation;
            Vector3 projectileLocation = Quaternion.Euler(0, rotation, 0) * transform.forward;
            GameObject projectile = Instantiate(projectilePrefab, enemy.position + projectileLocation, projectileRotation);
            projectile.GetComponent<Projectile>().Initialize(damage, projectileSpeed, projectileRange, 9, effects);
        }
    }
}