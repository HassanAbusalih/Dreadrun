using System.Collections;
using UnityEngine;

public class TurretWeapon : EnemyWeapon
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
            projectile.GetComponent<Projectile>().Initialize(damageModifier, projectileSpeed, projectileRange, 9);
            if (audioSource != null) audioSource.Play();
            timeSinceLastShot = 0;
            yield return new WaitForSeconds(fireRate);
        }
        timeSinceLastShot = 0;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color (0,1,0,0.2f);
        Gizmos.DrawSphere(transform.position, ProjectileRange);
    }
}
