using UnityEngine;

public class Shotgun : PlayerWeapon
{
    [Header("Shotgun Properties")]
    [SerializeField] int degreesPerProjectile = 5;
    
    private void Start()
    {
        timeSinceLastShot = fireRate;
    }
    
    private void Update()
    {
        if (!equipped) { return; }
        timeSinceLastShot += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Attack();
        }
    }

    public override void Attack()
    {
        if (timeSinceLastShot < fireRate) return;
        timeSinceLastShot = 0;
        int projectileCount = Mathf.Max(1, Mathf.FloorToInt(spreadAngle / degreesPerProjectile));
        float rotationAmount = spreadAngle / (projectileCount - 1);
        float startAngle = -spreadAngle / 2;
        for (int i = 0; i < projectileCount; i++)
        {
            float rotation = startAngle + i * rotationAmount;
            Quaternion projectileRotation = Quaternion.Euler(0, transform.eulerAngles.y + rotation, 0);
            Vector3 projectileLocation = Quaternion.Euler(0, rotation, 0) * transform.forward;
            GameObject projectile = Instantiate(projectilePrefab, transform.position + projectileLocation, projectileRotation);
            projectile.GetComponent<Projectile>().Initialize(damageModifier, projectileSpeed, projectileRange, 8);
        }
        if (audioSource != null) audioSource.Play();
    }
}