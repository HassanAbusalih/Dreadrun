using UnityEngine;

public class GenericWeapon : WeaponBase
{
    [SerializeField] int projectileCount = 3;
    [SerializeField] float projectileAngle;
    private void Start()
    {
        timeSinceLastShot = fireRate;
    }
    private void Update()
    {
        timeSinceLastShot += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Shoot();
        }
    }

    public override void Shoot()
    {
        if (timeSinceLastShot < fireRate) return;
        timeSinceLastShot = 0;
        float rotationAmount = projectileAngle / (projectileCount - 1);
        float startAngle = -projectileAngle / 2;
        for (int i = 0; i < projectileCount; i++)
        {
            float rotation = startAngle + i * rotationAmount;
            Quaternion projectileRotation = Quaternion.Euler(0, transform.eulerAngles.y + rotation, 0);
            Vector3 projectileLocation = Quaternion.Euler(0, rotation, 0) * transform.forward;
            GameObject projectile = Instantiate(projectilePrefab, transform.position + projectileLocation, projectileRotation);
            projectile.GetComponent<Projectile>().Initialize(damagePerShot, projectileSpeed, projectileRange);
        }
        if (audioSource != null) audioSource.Play();
    }
}
