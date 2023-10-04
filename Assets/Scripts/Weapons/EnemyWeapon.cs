using UnityEngine;

public class EnemyWeapon : WeaponBase
{
    [SerializeField] int projectileCount = 1;

    private void Start()
    {
        timeSinceLastShot = fireRate;
    }

    private void Update()
    {
        timeSinceLastShot += Time.deltaTime;
        if (timeSinceLastShot > fireRate)
        {
            timeSinceLastShot = 0;
            Shoot();
        }
    }

    public override void Shoot()
    {
        float rotationAmount = 360 / projectileCount;
        for (int i = 0; i < projectileCount; i++)
        {
            float rotation = i * rotationAmount;
            Quaternion projectileRotation = Quaternion.Euler(0, rotation, 0) * transform.rotation;
            Vector3 projectileLocation = Quaternion.Euler(0, rotation, 0) * transform.forward;
            GameObject projectile = Instantiate(projectilePrefab, transform.position + projectileLocation, projectileRotation);
            projectile.GetComponent<Projectile>().Initialize(damagePerShot, projectileSpeed, projectileRange, 9);
        }
        if (audioSource != null) audioSource.Play();
    }
}
