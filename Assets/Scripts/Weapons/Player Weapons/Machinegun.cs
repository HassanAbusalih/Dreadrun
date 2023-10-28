using UnityEngine;

public class Machinegun : PlayerWeapon
{
    [Header("Machinegun Properties")]
    [SerializeField] float rampTime = 5;
    [SerializeField] float rampDelay = 0.5f;
    float firingDuration;

    void Update()
    {
        if (!equipped) { return; }
        timeSinceLastShot += Time.deltaTime;
        firingDuration += Time.deltaTime;
        if (Input.GetKey(KeyCode.Mouse0))
        {
            Attack();
        }
        else
        {
            firingDuration = 0;
            timeSinceLastShot = 0;
        }
    }

    public override void Attack()
    {
        if (timeSinceLastShot < fireRate) return;
        timeSinceLastShot = 0;
        float currentSpread = 0;
        if (firingDuration > rampDelay)
        {
            currentSpread = Mathf.Lerp(0, spreadAngle, (firingDuration - rampDelay) / rampTime);
        }
        float randomAngle = Random.Range(-currentSpread, currentSpread);
        Quaternion projectileRotation = Quaternion.Euler(0, transform.eulerAngles.y + randomAngle, 0);
        GameObject projectile = Instantiate(projectilePrefab, BulletSpawnPoint.position + transform.forward, projectileRotation);
        projectile.GetComponent<Projectile>().Initialize(damageModifier, projectileSpeed, projectileRange, 8, effects);
        if (audioSource != null) audioSource.Play();
    }
}
