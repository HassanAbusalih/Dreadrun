using System;
using System.Collections.Generic;
using UnityEngine;

public class ScatterEffect : MonoBehaviour, IProjectileEffect
{
    int projectileCount = 5;

    public void ApplyEffect(IDamagable damagable, float damage, List<IProjectileEffect> projectileEffects)
    {
        projectileEffects.RemoveAll((effect) => EffectsToRemove().Contains(effect.GetType()));

    }

    public List<Type> EffectsToRemove()
    {
        return new List<Type> { typeof(ScatterEffect) };
    }

    void Scatter()
    {
        float rotationAmount = 360 / projectileCount;
        for (int i = 0; i < projectileCount; i++)
        {
            float rotation = i * rotationAmount;
            // Won't work currently since I don't have a reference to the projectile prefab or the transform of the enemy.
            //Quaternion projectileRotation = Quaternion.Euler(0, rotation, 0) * transform.rotation;
            //Vector3 projectileLocation = Quaternion.Euler(0, rotation, 0) * transform.forward;
            //GameObject projectile = Instantiate(projectilePrefab, transform.position + projectileLocation, projectileRotation);
            //projectile.GetComponent<Projectile>().Initialize(damageModifier, projectileSpeed, projectileRange, 9, effects);
        }
    }
}