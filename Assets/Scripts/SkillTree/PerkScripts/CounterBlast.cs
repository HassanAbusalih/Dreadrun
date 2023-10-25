using UnityEngine;

public class CounterBlast : MonoBehaviour
{
    private float explosionRadius = 4f;
    private float explosionForce = 4000f;

    private float explosionCooldown = 5f;
    private float explosionTimer;

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
        if(explosionTimer < explosionCooldown)
        {
            return;
        }
        explosionTimer = 0;

        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider nearbyObject in colliders)
        {
            if(nearbyObject.gameObject == gameObject)
            {
                continue;
            }

            if (nearbyObject.TryGetComponent(out Rigidbody rb))
            {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius, 3.0f);
            }

            if (nearbyObject.TryGetComponent(out IDamagable newDamagable))
            {
                newDamagable.TakeDamage(damage);
            }
        }
    }
}