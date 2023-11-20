using UnityEngine;
using UnityEngine.UI;

public class Bow : PlayerWeapon
{
    [Header("Bow Properties")]
    [SerializeField] float homingRange = 5;
    [SerializeField] float homingStrength = 250;
    [SerializeField] float minDamageModifier = 10;
    [SerializeField] float minRange = 10;
    [SerializeField] float minSpeed = 5;
    [SerializeField] float maxProjectiles = 3;
    [SerializeField] Image chargeIndicator;
    [SerializeField] Color minColor;
    [SerializeField] Color maxColor;
    float chargeTime = 0;

    bool hasPlayedWindUpSFX = false;

    void Update()
    {
        if (!equipped) { return; }
        if (Input.GetKey(KeyCode.Mouse0) || Input.GetButton("shoot"))
        {
            chargeTime = Mathf.Min(chargeTime + Time.deltaTime, fireRate);
            if (!hasPlayedWindUpSFX && soundSO != null)
            {
                soundSO.PlaySound(3, AudioSourceType.Weapons);
                hasPlayedWindUpSFX = true;
            }

        }
        else if (Input.GetKeyUp(KeyCode.Mouse0) || Input.GetButtonUp("shoot"))
        {
            Attack();
            chargeTime = 0;
            hasPlayedWindUpSFX = false;
        }
        VisualFeedback();
    }

    public override void Attack()
    {
        float currentDamage = Mathf.Lerp(minDamageModifier, damageModifier, chargeTime / fireRate);
        float currentRange = Mathf.Lerp(minRange, projectileRange, chargeTime / fireRate);
        float currentSpeed = Mathf.Lerp(minSpeed, projectileSpeed, chargeTime / fireRate);
        int projectilesToSpawn = Mathf.RoundToInt(Mathf.Lerp(1, maxProjectiles, chargeTime / fireRate));
        float rotationAmount = spreadAngle / (projectilesToSpawn - 1);
        float startAngle = -spreadAngle / 2;
        Debug.Log(currentSpeed);
        for (int i = 0; i < projectilesToSpawn; i++)
        {
            float rotation = projectilesToSpawn > 1 ? startAngle + i * rotationAmount : 0;
            Quaternion projectileRotation = Quaternion.Euler(0, transform.eulerAngles.y + rotation, 0);
            Vector3 projectileLocation = Quaternion.Euler(0, rotation, 0) * transform.forward;
            GameObject projectile = Instantiate(projectilePrefab, BulletSpawnPoint.position + projectileLocation, projectileRotation);
            projectile.AddComponent<Homing>().Initialize(homingStrength, homingRange);
            projectile.GetComponent<Projectile>().Initialize(currentDamage, currentSpeed, currentRange, 8, effects);
        }
        if (soundSO != null)
        {
            soundSO.PlaySound(2, AudioSourceType.Weapons);
        }
    }

    void VisualFeedback()
    {
        if (chargeTime > 0)
        {
            chargeIndicator.gameObject.SetActive(true);
            chargeIndicator.fillAmount = chargeTime / fireRate;
            chargeIndicator.color = Color.Lerp(minColor, maxColor, chargeIndicator.fillAmount);
        }
        else
        {
            chargeIndicator.gameObject.SetActive(false);
            chargeIndicator.fillAmount = 0;
            chargeIndicator.color = minColor;
        }
    }
}