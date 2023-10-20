using UnityEngine;

public class Grenade : MonoBehaviour
{
    [SerializeField] float explosionRadius = 5f;
    [SerializeField] float explosionForce = 700f;
    [SerializeField] float explosionDelay = 3;
    [SerializeField] int ignoreLayer;
    Vector3 target;
    float speed;
    float damage;

    public void Initialize(Vector3 target, float speed, float damage, int ignoreLayer)
    {
        this.target = target;
        this.speed = speed;
        this.damage = damage;
        this.ignoreLayer = ignoreLayer;
    }

    private void Update()
    {
        // some bezier curve logic here I guess
        // Explode(); when reached target
    }

    void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider nearbyObject in colliders)
        {
            if (nearbyObject.gameObject.layer == ignoreLayer) continue;
            if (nearbyObject.TryGetComponent(out Rigidbody rb))
            {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }
            if (nearbyObject.TryGetComponent(out IDamagable damagable))
            {
                damagable.TakeDamage(damage);
            }
        }
        Destroy(gameObject);
    }
}
