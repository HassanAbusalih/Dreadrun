using UnityEngine;

public class Machinegun : PlayerWeapon
{
    [Header("Machinegun Properties")]
    [SerializeField] float rampTime = 5;
    [SerializeField] float rampDelay = 0.5f;
    [SerializeField] float firingDuration;

    void Update()
    {
        if (!equipped) { return; }
        timeSinceLastShot += Time.deltaTime;
        if (Input.GetKey(KeyCode.Mouse0) || Input.GetButton("shoot"))
        {
            if (timeSinceLastShot >= fireRate)
            {
                Attack();
                Debug.Log("MachineGun attacked");
                timeSinceLastShot = 0;
            }
            firingDuration += Time.deltaTime;
        }
        else
        {
            firingDuration -= 2 * Time.deltaTime;
        }
        firingDuration = Mathf.Clamp(firingDuration, 0, rampTime + rampDelay);
    }

    public override void Attack()
    {
        float currentSpread = 0;
        if (firingDuration > rampDelay)
        {
            currentSpread = Mathf.Lerp(0, spreadAngle, (firingDuration - rampDelay) / rampTime);
        }
        float randomAngle = Random.Range(-currentSpread, currentSpread);
        Quaternion projectileRotation = Quaternion.Euler(projectilePrefab.transform.eulerAngles.x, transform.eulerAngles.y + randomAngle, 0);
        //GameObject projectile = Instantiate(projectilePrefab, BulletSpawnPoint.position, projectileRotation);
        GameObject projectile = ObjectPooling.instance.GetPooledObject();
        projectile.transform.position = BulletSpawnPoint.position;
        projectile.transform.rotation = projectileRotation;
        projectile.SetActive(true);
        projectile.GetComponent<Projectile>().Initialize(damageModifier, projectileSpeed, projectileRange, 8, effects);
        if (soundSO != null) soundSO.PlaySound(2, AudioSourceType.Weapons);
        if (impulseSource != null) impulseSource.GenerateImpulse();
    }
}