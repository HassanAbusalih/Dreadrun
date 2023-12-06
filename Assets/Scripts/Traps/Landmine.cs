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
        Explosion.Explode(transform, explosionDamage, explosionRadius, explosionForce);
        Instantiate(explosionVFX, transform.position, Quaternion.identity);
        if (explosionSFX != null) { explosionSFX.PlaySound(0, AudioSourceType.Weapons); }
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

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0.92f, 0.016f, 0.2f);
        Gizmos.DrawSphere(transform.position, explosionRadius);
    }
}