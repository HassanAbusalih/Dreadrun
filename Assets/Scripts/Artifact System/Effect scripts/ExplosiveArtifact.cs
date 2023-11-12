using System;
using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

[Serializable]
public class ExplosiveArtifact : Artifact
{
    private bool canExplode = true;

    ExplosiveArtifactSettings ArtifactSettings => (ExplosiveArtifactSettings)base.settings;
    private float TotalExplosionDamage => level * ArtifactSettings.explosionDamagePerLevel;
    private float TotalPushBackForce => level * ArtifactSettings.pushBackForcePerLevel;

    public override void Initialize()
    {
        this.prefab = ArtifactSettings.artifactPrefab;
        ArtifactSettings.ShockwaveEffect = prefab.GetComponentInChildren<VisualEffect>();
    }

    public override void ApplyArtifactEffects()
    {
        if (!canExplode) return;

        Collider[] enemiesInRange = Physics.OverlapSphere(manager.artifactGameObject.transform.position, ArtifactSettings.explosionRadius, manager.enemyLayer);

        if (enemiesInRange.Length < ArtifactSettings.enemiesRequired) return;

        foreach (Collider enemyInRange in enemiesInRange)
        {
            Rigidbody rb = enemyInRange.GetComponent<Rigidbody>();
            IDamagable enemyDamagable = enemyInRange.GetComponent<IDamagable>();

            if (rb != null)
            {
                rb.AddExplosionForce(TotalPushBackForce, manager.artifactPosition, ArtifactSettings.explosionRadius, 0, ForceMode.Impulse);
                ArtifactSettings.ShockwaveEffect.Play();
            }

            if (enemyDamagable != null)
            {
                enemyDamagable.TakeDamage(TotalExplosionDamage);
            }
        }

        canExplode = false;
        manager.StartCoroutine(ExplosionCooldown());
    }

    private IEnumerator ExplosionCooldown()
    {
        yield return new WaitForSeconds(ArtifactSettings.explosionCooldown);
        canExplode = true;
    }
}
