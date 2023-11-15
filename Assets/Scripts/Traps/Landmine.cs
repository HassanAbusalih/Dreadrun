using UnityEngine;

public class Landmine : Trap
{
    [SerializeField] float explosionRadius = 5f;
    [SerializeField] float explosionDamage = 20f;
    [SerializeField] float explosionForce = 700;
    [SerializeField] GameObject explosionVFX;
    [SerializeField] SoundSO explosionSFX;

    private void Update()
    {
        if (playerCount > 0)
        {
            timer += Time.deltaTime;
            if (timer > trapDelay)
            {
                Explode();
            }
        }
        else
        {
            timer = 0;
        }
    }

    private void Explode()
    {
        Instantiate(explosionVFX, transform.position, Quaternion.identity);
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider nearbyObject in colliders)
        {
            if (nearbyObject.TryGetComponent(out Rigidbody rb))
            {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius, 0, ForceMode.Impulse);
            }
            if (nearbyObject.TryGetComponent(out IDamagable newDamagable))
            {
                newDamagable.TakeDamage(explosionDamage);
            }
        }
        if (explosionSFX != null) { explosionSFX.PlaySound(0, AudioSourceType.Environment); }
        Destroy(gameObject);
    }

    protected new void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (other.TryGetComponent(out Projectile _) || other.TryGetComponent(out Grenade _))
        {
            Explode();
        }
    }
}