using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Explosive Artifact Object", menuName = "ArtifactsEffects/Explosive Artifact")]
public class ExplosiveArtifact : Artifact
{
    [SerializeField] private float explosionDamagePerLevel;
    [SerializeField] private float pushBackForcePerLevel;
    [SerializeField] private float explosionRadius;
    [SerializeField] private int enemiesRequired;
    [SerializeField] private int explosionCooldown;
    [SerializeField] private LayerMask enemyLayer;

    private bool canExplode = true;

    private float TotalExplosionDamage => level * explosionDamagePerLevel;

    private float TotalPushBackForce => level * pushBackForcePerLevel;

    public override void InitializeArtifact()
    {
        canExplode = true;
    }

    public override void ApplyArtifactBuffs(Vector3 artifactPosition, float effectRange, ArtifactManager manager)
    {
        if (!canExplode) return;

        Collider[] enemiesInRange = Physics.OverlapSphere(artifactPosition, effectRange, enemyLayer);

        if (enemiesInRange.Length < enemiesRequired) return;

        foreach (Collider enemy in enemiesInRange)
        {
            Rigidbody rb = enemy.GetComponent<Rigidbody>();
            IDamagable enemyDamagable = enemy.GetComponent<IDamagable>();

            if (rb != null)
            {
                rb.AddExplosionForce(TotalPushBackForce, artifactPosition, explosionRadius, 0, ForceMode.Impulse);
            }
            if (enemyDamagable != null)
            {
                enemyDamagable.TakeDamage(TotalExplosionDamage);
            }
        }

        canExplode = false;
        manager.StartCoroutine(ResetExplosion());
    }

    private IEnumerator ResetExplosion()
    {
        yield return new WaitForSeconds(explosionCooldown);
        canExplode = true;
    }
}
