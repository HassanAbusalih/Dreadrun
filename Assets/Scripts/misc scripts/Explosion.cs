using System.Collections.Generic;
using UnityEngine;

public static class Explosion
{
    public static void Explode(Transform origin, float damage, float explosionRadius, float explosionForce)
    {
        Collider[] colliders = Physics.OverlapSphere(origin.position, explosionRadius);
        foreach (Collider nearbyObject in colliders)
        {
            if (nearbyObject.transform == origin) { continue; }
            if (nearbyObject.TryGetComponent(out Rigidbody rb))
            {
                Vector3 explosionDirection = (nearbyObject.transform.position - origin.position).normalized;
                explosionDirection.y = 0;
                rb.AddForce(explosionDirection * explosionForce, ForceMode.Impulse);
            }
            if (nearbyObject.TryGetComponent(out IDamagable damagable))
            {
                damagable.TakeDamage(damage);
            }
        }
    }

    public static void Explode(Transform origin, float damage, float explosionRadius, float explosionForce, LayerMask layersToIgnore, List<IProjectileEffect> effects)
    {
        Collider[] colliders = Physics.OverlapSphere(origin.position, explosionRadius, ~layersToIgnore);
        foreach (Collider nearbyObject in colliders)
        {
            if (nearbyObject.transform == origin) { continue; }
            if (nearbyObject.TryGetComponent(out Rigidbody rb))
            {
                Vector3 explosionDirection = (nearbyObject.transform.position - origin.position).normalized;
                explosionDirection.y = 0;
                rb.AddForce(explosionDirection * explosionForce, ForceMode.Impulse);
            }
            if (nearbyObject.TryGetComponent(out IDamagable damagable))
            {
                damagable.TakeDamage(damage);
                if (effects != null && !damagable.gameObject.TryGetComponent(out PayloadStats payload))
                {
                    foreach (IProjectileEffect effect in effects)
                    {
                        effect.ApplyEffect(damagable, damage, new List<IProjectileEffect>(effects));
                    }
                }
            }
        }
    }

    public static void Explode(Transform origin, float damage, float explosionRadius, float explosionForce, LayerMask layersToIgnore)
    {
        Collider[] colliders = Physics.OverlapSphere(origin.position, explosionRadius, ~layersToIgnore);
        foreach (Collider nearbyObject in colliders)
        {
            if (nearbyObject.transform == origin) { continue; }
            if (nearbyObject.TryGetComponent(out Rigidbody rb))
            {
                Vector3 explosionDirection = (nearbyObject.transform.position - origin.position).normalized;
                explosionDirection.y = 0;
                rb.AddForce(explosionDirection * explosionForce, ForceMode.Impulse);
            }
            if (nearbyObject.TryGetComponent(out IDamagable damagable))
            {
                damagable.TakeDamage(damage);
            }
        }
    }

    public static void Explode(Transform origin, float damage, float explosionRadius, float explosionForce, ForceMode forceMode)
    {
        Collider[] colliders = Physics.OverlapSphere(origin.position, explosionRadius);
        foreach (Collider nearbyObject in colliders)
        {
            if (nearbyObject.transform == origin) { continue; }
            if (nearbyObject.TryGetComponent(out Rigidbody rb))
            {
                Vector3 explosionDirection = (nearbyObject.transform.position - origin.position).normalized;
                explosionDirection.y = 0;
                rb.AddForce(explosionDirection * explosionForce, forceMode);
            }
            if (nearbyObject.TryGetComponent(out IDamagable damagable))
            {
                damagable.TakeDamage(damage);
            }
        }
    }
}
