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
        if (Input.GetKey(KeyCode.Mouse0) || Input.GetButton("shoot"))
        {
            if (timeSinceLastShot >= fireRate)
            {
                Attack();
                timeSinceLastShot = 0;
            }
            firingDuration += Time.deltaTime;
        }
        else
        {
            firingDuration -= Time.deltaTime;
            if (firingDuration < 0 ) { firingDuration = 0; }
        }
    }

    public override void Attack()
    {
        float currentSpread = 0;
        if (firingDuration > rampDelay)
        {
            currentSpread = Mathf.Lerp(0, spreadAngle, (firingDuration - rampDelay) / rampTime);
        }
        float randomAngle = Random.Range(-currentSpread, currentSpread);
        Quaternion projectileRotation = Quaternion.Euler(0, transform.eulerAngles.y + randomAngle, 0);
        GameObject projectile = Instantiate(projectilePrefab, BulletSpawnPoint.position + transform.forward, projectileRotation);
        projectile.GetComponent<Projectile>().Initialize(damageModifier, projectileSpeed, projectileRange, 8, effects);
        soundSO.PlaySound(2, AudioSourceType.Weapons);
    }
}