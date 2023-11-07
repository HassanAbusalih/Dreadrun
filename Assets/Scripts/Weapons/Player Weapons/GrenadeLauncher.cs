using UnityEngine;
using UnityEngine.UI;

public class GrenadeLauncher : PlayerWeapon
{
    [Header("Grenade Launcher Properties")]
    [SerializeField] float minRange = 5;
    [SerializeField] float minSpeed = 5;
    [SerializeField] Image chargeIndicator;
    [SerializeField] Color minColor;
    [SerializeField] Color maxColor;
    public float chargeTime = 0;
    [SerializeField] SoundSO grenadeLauncherShotSFX;

    private void Update()
    {
        if (!equipped) { return; }
        if (Input.GetKey(KeyCode.Mouse0))
        {
            chargeTime = Mathf.Min(chargeTime + Time.deltaTime, fireRate);
          
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            Attack();
            grenadeLauncherShotSFX.Play();
            chargeTime = 0;
        }
        if (chargeIndicator != null) { VisualFeedback(); }
    }

    public override void Attack()
    {
        float currentRange = Mathf.Lerp(minRange, projectileRange, chargeTime / fireRate);
        float currentSpeed = Mathf.Lerp(minSpeed, projectileRange, chargeTime / fireRate);
        RaycastHit hit;
        Vector3 target;
        if (Physics.Raycast(transform.position, transform.forward, out hit, currentRange))
        {
            target = hit.point;
        }
        else
        {
            Vector3 endPoint = transform.position + transform.forward * currentRange;
            if (Physics.Raycast(endPoint, Vector3.down, out hit))
            {
                target = hit.point;
            }
            else
            {
                target = endPoint;
            }
            // This is so bad aaaaaaa kill me
        }
        GameObject projectile = Instantiate(projectilePrefab, BulletSpawnPoint.position + transform.forward, transform.rotation);
        projectile.GetComponent<Grenade>().Initialize(target, currentSpeed, damageModifier, 8, effects);
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

    public override void UpdateWeaponEffects()
    {
        base.UpdateWeaponEffects();
        effects.RemoveAll((effect) => effect is ExplosiveEffect);
    }
}