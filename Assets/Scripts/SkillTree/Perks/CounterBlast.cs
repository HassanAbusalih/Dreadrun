using Unity.Mathematics;
using UnityEngine;

public class CounterBlast : MonoBehaviour
{
    private float explosionRadius = 4f;
    private float explosionForce = 4000f;

    private float explosionCooldown = 5f;
    private float explosionTimer;
    private LayerMask layersToIgnore;
    private GameObject vfx;

    private void Start()
    {
        explosionTimer = explosionCooldown;
    }

    private void Update()
    {
        if (explosionTimer < explosionCooldown)
        {
            explosionTimer += Time.deltaTime;
        }
    }

    public void Explode(float damage)
    {
        if (explosionTimer < explosionCooldown)
        {
            return;
        }
        explosionTimer = 0;

        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius, ~layersToIgnore);
        foreach (Collider nearbyObject in colliders)
        {
            if (nearbyObject.gameObject == gameObject)
            {
                continue;
            }
            if (nearbyObject.TryGetComponent(out Rigidbody rb))
            {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius, 0, ForceMode.Impulse);
            }

            if (nearbyObject.TryGetComponent(out IDamagable newDamagable))
            {
                newDamagable.TakeDamage(damage);
            }
        }
        Instantiate(vfx, transform.position, transform.rotation);
    }
    public void SetCounterBlast(float radius, float force, LayerMask layerMask, GameObject vfx, float cooldown)
    {
        explosionRadius = radius;
        explosionForce = force;
        layersToIgnore = layerMask;
        this.vfx = vfx;
        explosionCooldown = cooldown;
    }
}