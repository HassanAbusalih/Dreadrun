using UnityEngine;
using UnityEngine.UI;

public class Bow : PlayerWeapon
{
    [Header("Bow Properties")]
    [SerializeField] float minDamageModifier = 10f;
    [SerializeField] float minRange = 10f;
    [SerializeField] float minSpeed = 5f;
    [SerializeField] Image chargeIndicator;
    [SerializeField] Color minColor;
    [SerializeField] Color maxColor;
    float chargeTime = 0;

    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            chargeTime = Mathf.Min(chargeTime + Time.deltaTime, fireRate);
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            Attack();
            chargeTime = 0f;
        }
        VisualFeedback();
    }

    public override void Attack()
    {
        float currentDamage = Mathf.Lerp(minDamageModifier, damageModifier, chargeTime / fireRate);
        float currentRange = Mathf.Lerp(minRange, projectileRange, chargeTime / fireRate);
        float currentSpeed = Mathf.Lerp(minSpeed, projectileRange, chargeTime / fireRate);
        Quaternion targetRotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3 (0, 90, 0)); // change this later 
        GameObject projectile = Instantiate(projectilePrefab, transform.position + transform.forward, targetRotation);
        projectile.GetComponent<Projectile>().Initialize(currentDamage, currentSpeed, currentRange, 8);
        if (audioSource != null) audioSource.Play();
    }

    void VisualFeedback()
    {
        if (chargeTime > 0)
        {
            chargeIndicator.fillAmount = chargeTime / fireRate;
            chargeIndicator.color = Color.Lerp(minColor, maxColor, chargeIndicator.fillAmount);
        }
        else
        {
            chargeIndicator.fillAmount = 0;
            chargeIndicator.color = minColor;
        }
    }
}
