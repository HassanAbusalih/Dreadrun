using System.Collections;
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
        if (CoroutineWorkHorse.instance == null)
        {
            (new GameObject("CoroutineWorkHorse")).AddComponent<CoroutineWorkHorse>();
        }
        CoroutineWorkHorse.instance.StartWork(ApplyForce(rb, rbPosition, explosionOrigin, explosionRadius, explosionForce, additionalExplosionForce, forceMultiplier));
    }

    static IEnumerator ApplyForce(Rigidbody rb, Vector3 rbPosition, Vector3 explosionOrigin, float explosionRadius, float explosionForce, float additionalExplosionForce, float forceMultiplier)
    {
        float timer = 0;
        while (timer < 0.1f && rb != null)
        {
            timer += Time.deltaTime;
            Vector3 direction = (rbPosition - explosionOrigin).normalized;
            direction.y = 0;
            rb.AddForce(direction * explosionForce, ForceMode.Force);
            yield return null;
        }
    }
}

public class CoroutineWorkHorse : MonoBehaviour
{
    public static CoroutineWorkHorse instance;
    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
    }

    public void StartWork(IEnumerator coroutine)
    {
        StartCoroutine(coroutine);
    }
}
