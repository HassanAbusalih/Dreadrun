using UnityEngine;

public class FodderWeapon : EnemyWeapon
{
    [SerializeField] int projectileCount = 1;
    Animation _animation;

    private void Update()
    {
        timeSinceLastShot += Time.deltaTime;
        _animation = GetComponent<Animation>();
    }

    public override void Attack()
    {
        if (timeSinceLastShot < fireRate) { return; }
        _animation.Play("Attack");
        _animation.Blend("Idle", 1, 0.5f);
        timeSinceLastShot = 0;
        float rotationAmount = 360 / projectileCount;
        for (int i = 0; i < projectileCount; i++)
        {
            float rotation = i * rotationAmount;
            Quaternion projectileRotation = Quaternion.Euler(0, rotation, 0) * transform.rotation;
            Vector3 projectileLocation = Quaternion.Euler(0, rotation, 0) * transform.forward;
            GameObject projectile = Instantiate(projectilePrefab, transform.position + projectileLocation, projectileRotation);
            projectile.GetComponent<Projectile>().Initialize(damageModifier, projectileSpeed, projectileRange, 9);
        }
        if (audioSource != null) audioSource.Play();
    }
}
