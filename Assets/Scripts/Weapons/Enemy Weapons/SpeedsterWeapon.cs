using UnityEngine;

public class SpeedsterWeapon : EnemyWeapon
{
    [SerializeField] float homingStrength = 150;
    [SerializeField] float homingRange = 10;
    public override void Attack()
    {
        GameObject projectile = Instantiate(projectilePrefab, transform.position + transform.forward, transform.rotation);
        projectile.AddComponent<Homing>().Initialize(homingStrength, homingRange, true);
        projectile.GetComponent<Projectile>().Initialize(damageModifier, projectileSpeed, projectileRange, 9);
        if (soundSO != null)
        {
            soundSO.PlaySound(0, AudioSourceType.Weapons);
        }
    }
}