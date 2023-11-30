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
            if (nearbyObject.TryGetComponent(out IDamagable damagable))
            {
                damagable.TakeDamage(damage);
            }
            if (nearbyObject.TryGetComponent(out Projectile _) || nearbyObject.TryGetComponent(out Grenade _)) { continue; }
            if (nearbyObject.TryGetComponent(out Rigidbody rb))
            {
                ApplyForces(rb, rb.transform.position, origin.position, explosionRadius, explosionForce, 2f, 0.01f);
            }
        }
    }

    public static void Explode(Transform origin, float damage, float explosionRadius, float explosionForce, LayerMask layersToIgnore, List<IProjectileEffect> effects)
    {
        Collider[] colliders = Physics.OverlapSphere(origin.position, explosionRadius, ~layersToIgnore);
        foreach (Collider nearbyObject in colliders)
        {
            if (nearbyObject.transform == origin) { continue; }
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
            if (nearbyObject.TryGetComponent(out Projectile _) || nearbyObject.TryGetComponent(out Grenade _)) { continue; }
            if (nearbyObject.TryGetComponent(out Rigidbody rb))
            {
                ApplyForces(rb, rb.transform.position, origin.position, explosionRadius, explosionForce, 2f, 0.01f);
            }
        }
    }

    public static void Explode(Vector3 point, float damage, float explosionRadius, float explosionForce, LayerMask layersToIgnore, List<IProjectileEffect> effects)
    {
        Collider[] colliders = Physics.OverlapSphere(point, explosionRadius, ~layersToIgnore);
        foreach (Collider nearbyObject in colliders)
        {
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
            if (nearbyObject.TryGetComponent(out Projectile _) || nearbyObject.TryGetComponent(out Grenade _)) { continue; }
            if (nearbyObject.TryGetComponent(out Rigidbody rb))
            {
                ApplyForces(rb, rb.transform.position, point, explosionRadius, explosionForce, 2f, 0.01f);
            }
        }
    }

    public static void Explode(Transform origin, float damage, float explosionRadius, float explosionForce, LayerMask layersToIgnore)
    {
        Collider[] colliders = Physics.OverlapSphere(origin.position, explosionRadius, ~layersToIgnore);
        foreach (Collider nearbyObject in colliders)
        {
            if (nearbyObject.transform == origin) { continue; }
            if (nearbyObject.TryGetComponent(out IDamagable damagable))
            {
                damagable.TakeDamage(damage);
            }
            if (nearbyObject.TryGetComponent(out Projectile _) || nearbyObject.TryGetComponent(out Grenade _)) { continue; }
            if (nearbyObject.TryGetComponent(out Rigidbody rb))
            {
                ApplyForces(rb, rb.transform.position, origin.position, explosionRadius, explosionForce, 2f, 0.01f);
            }
        }
    }

    public static void Explode(Transform origin, float damage, float explosionRadius, float explosionForce, ForceMode forceMode)
    {
        Collider[] colliders = Physics.OverlapSphere(origin.position, explosionRadius);
        foreach (Collider nearbyObject in colliders)
        {
            if (nearbyObject.transform == origin) { continue; }
            if (nearbyObject.TryGetComponent(out IDamagable damagable))
            {
                damagable.TakeDamage(damage);
            }
            if (nearbyObject.TryGetComponent(out Projectile _) || nearbyObject.TryGetComponent(out Grenade _)) { continue; }
            if (nearbyObject.TryGetComponent(out Rigidbody rb))
            {
                ApplyForces(rb, rb.transform.position, origin.position, explosionRadius, explosionForce, 2f, 0.01f);
            }
        }
    }

    public static void ApplyForces(Rigidbody rb, Vector3 rbPosition, Vector3 explosionOrigin, float explosionRadius, float explosionForce, float additionalExplosionForce, float forceMultiplier)
    {
        Vector3 knockbackDirection = (rbPosition - explosionOrigin).normalized;
        knockbackDirection.y = 0.2f;

        Vector3 biasedKnockbackDirection = knockbackDirection + Vector3.up * 0.9f;

        float smoothing = 0.5f;
        Vector3 targetVelocity = Vector3.Lerp(rb.velocity, biasedKnockbackDirection * explosionForce, smoothing);
        rb.velocity = targetVelocity;

        rb.AddExplosionForce(additionalExplosionForce, explosionOrigin, explosionRadius);
        rb.AddForce(biasedKnockbackDirection * explosionForce * forceMultiplier, ForceMode.Impulse);
    }
}
