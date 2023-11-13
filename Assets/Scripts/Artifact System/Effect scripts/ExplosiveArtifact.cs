using System;
using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

[Serializable]
public class ExplosiveArtifact : Artifact
{
    private bool canExplode = true;
    VisualEffect ShockwaveEffect;

    // Cast the value of inherited settings variable to the correct settings type and assign it to a variable for easier use
    ExplosiveArtifactSettings ArtifactSettings => (ExplosiveArtifactSettings)base.settings;
    private float TotalExplosionDamage => level * ArtifactSettings.explosionDamagePerLevel;
    private float TotalPushBackForce => level * ArtifactSettings.pushBackForcePerLevel;

    public override void Initialize()
    {
        ShockwaveEffect = gameObject.GetComponentInChildren<VisualEffect>();
    }

    public override void ApplyArtifactEffects()
    {
        if (!canExplode) return;

        Collider[] enemiesInRange = GetEnemiesInRange();

        if (enemiesInRange.Length < ArtifactSettings.enemiesRequired) return;

        ApplyExplosionEffect(enemiesInRange);

        canExplode = false;
        manager.StartCoroutine(ExplosionCooldown());
    }

    private Collider[] GetEnemiesInRange()
    {
        return Physics.OverlapSphere(manager.artifactGameObject.transform.position, ArtifactSettings.explosionRadius, manager.enemyLayer);
    }

    private void ApplyExplosionEffect(Collider[] enemiesInRange)
    {
        foreach (Collider enemyInRange in enemiesInRange)
        {
            Rigidbody rb = enemyInRange.GetComponent<Rigidbody>();
            IDamagable enemyDamagable = enemyInRange.GetComponent<IDamagable>();

            if (rb != null)
            {
                rb.AddExplosionForce(TotalPushBackForce, manager.transform.position, ArtifactSettings.explosionRadius, 0, ForceMode.Impulse);
                ShockwaveEffect.Play();
            }

            if (enemyDamagable != null)
            {
                enemyDamagable.TakeDamage(TotalExplosionDamage);
            }
        }
    }

    private IEnumerator ExplosionCooldown()
    {
        yield return new WaitForSeconds(ArtifactSettings.explosionCooldown);
        canExplode = true;
    }
}
