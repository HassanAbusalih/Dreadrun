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

    void Update()
    {
        if (!equipped) { return; }
        if (Input.GetKey(KeyCode.Mouse0))
        {
            chargeTime = Mathf.Min(chargeTime + Time.deltaTime, fireRate);
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            Attack();
            chargeTime = 0;
        }
        VisualFeedback();
    }

    public override void Attack()
    {
        float currentDamage = Mathf.Lerp(minDamageModifier, damageModifier, chargeTime / fireRate);
        float currentRange = Mathf.Lerp(minRange, projectileRange, chargeTime / fireRate);
        float currentSpeed = Mathf.Lerp(minSpeed, projectileRange, chargeTime / fireRate);
        GameObject projectile = Instantiate(projectilePrefab, transform.position + transform.forward, transform.rotation);
        projectile.GetComponent<Projectile>().Initialize(currentDamage, currentSpeed, currentRange, 8, effects);
        if (audioSource != null) audioSource.Play();
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