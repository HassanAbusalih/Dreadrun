using System.Collections;
using UnityEngine;

public class TurretWeapon : EnemyWeapon
{
    [SerializeField] int projectileCount = 1;
    [SerializeField] Transform projectileSpawnPoint;
    [SerializeField] float cooldown;


    private void Update()
    {
        timeSinceLastShot += Time.deltaTime;
    }

    public void Attack(Transform target)
    {
        if (timeSinceLastShot < cooldown) { return; }
        StartCoroutine(Shoot(target));
    }

    IEnumerator Shoot(Transform target)
    {
        int counter = 0;
        while (counter < projectileCount)
        {
            counter++;
            projectileSpawnPoint.LookAt(target);
            GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
            projectile.GetComponent<Projectile>().Initialize(damageModifier, projectileSpeed, projectileRange, 9);
            if (soundSO != null) soundSO.PlaySound(0, AudioSourceType.EnemyShoot);
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

    public override void Attack()
    {
        throw new System.NotImplementedException();
    }
}
