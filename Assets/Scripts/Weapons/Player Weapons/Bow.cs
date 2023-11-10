using UnityEngine;
using UnityEngine.UI;

public class Bow : PlayerWeapon
{
    [Header("Bow Properties")]
    [SerializeField] float minDamageModifier = 10;
    [SerializeField] float minRange = 10;
    [SerializeField] float minSpeed = 5;
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
            if (!hasPlayedWindUpSFX)
            {
                soundSO.PlaySound(ref audioSource, 3, this.gameObject);
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
        float currentSpeed = Mathf.Lerp(minSpeed, projectileRange, chargeTime / fireRate);
        GameObject projectile = Instantiate(projectilePrefab, BulletSpawnPoint.position + transform.forward, transform.rotation);
        projectile.GetComponent<Projectile>().Initialize(currentDamage, currentSpeed, currentRange, 8, effects);
        soundSO.PlaySound(ref audioSource, 2, this.gameObject);
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