using UnityEngine;

public class FodderWeapon : EnemyWeapon
{
    [SerializeField] int projectileCount = 1;
    Animation anim;

    private void Update()
    {
        timeSinceLastShot += Time.deltaTime;
        anim = GetComponentInChildren<Animation>();
    }

    public override void Attack()
    {
        if (timeSinceLastShot < fireRate) { return; }
        anim.CrossFade("Attack", 0.2f);
        float attackDuration = 0.7f;
        float fadeOutTime = 0.1f;
        Invoke(nameof(ReturnToIdle), attackDuration - fadeOutTime);
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

    void ReturnToIdle()
    {
        anim.CrossFade("Idle", 0.5f);
    }
}
