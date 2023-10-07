using System.Collections;
using UnityEngine;

public class TurretWeapon : WeaponBase
{
    [SerializeField] int projectileCount = 1;
    [SerializeField] float cooldown;

    private void Update()
    {
        timeSinceLastShot += Time.deltaTime;
    }

    public override void Attack()
    {
        if (timeSinceLastShot < cooldown) { return; }
        StartCoroutine(Shoot());
    }

    IEnumerator Shoot()
    {
        int counter = 0;
        while (counter < projectileCount)
        {
            counter++;
            GameObject projectile = Instantiate(projectilePrefab, transform.position + transform.forward, transform.rotation);
            projectile.GetComponent<Projectile>().Initialize(damagePerShot, projectileSpeed, projectileRange, 9);
            if (audioSource != null) audioSource.Play();
            timeSinceLastShot = 0;
            yield return new WaitForSeconds(fireRate);
        }
        timeSinceLastShot = 0;
    }
}
