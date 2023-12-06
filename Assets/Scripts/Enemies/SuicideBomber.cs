using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SuicideBomber : EnemyAIBase
{
    [SerializeField] float explosionDamage = 10f;
    [SerializeField] float explosionRadius = 5f;
    [SerializeField] float explosionForce = 700f;
    [SerializeField] float dashSpeedModifier = 5f;
    [SerializeField] float dashRange = 5f;
    [SerializeField] float explodeRange = 2f;
    [SerializeField] GameObject explosionVFX;
    bool dashing = false;
    LayerMask mask;
    [SerializeField] SkinnedMeshRenderer[] meshRenderer;
    List<Material> materials = new();

    private void Start()
    {
        foreach (SkinnedMeshRenderer mesh in meshRenderer)
        {
            materials.Add(mesh.material);
        }
        mask = LayerMask.GetMask("Enemy");
    }

    private void Update()
    {
        if (payload == null)
        {
            target = GetClosestPlayer();
        }
        else
        {
            target = payload;
        }
        if (target == null)
        {
            rb.velocity = new(0f, rb.velocity.y, 0f);
            return;
        }

        float distanceToTarget = Vector3.Distance(transform.position, target.position);
        if (!dashing && distanceToTarget <= dashRange && distanceToTarget > explodeRange)
        {
            dashing = true;
        }

        if (dashing)
        {
            Move(target, movementSpeed * dashSpeedModifier);
            float lerp = Mathf.Clamp01((dashRange - distanceToTarget) / (dashRange - explodeRange));
            foreach  (Material material in materials)
            {
                material.color = Color.Lerp(Color.white, Color.red, lerp);
            }
        }
        else
        {
            Move(target, movementSpeed);
        }

        if (distanceToTarget <= explodeRange)
        {
            Explosion.Explode(transform, explosionDamage, explosionRadius, explosionForce, mask);
            Instantiate(explosionVFX, transform.position, Quaternion.identity);
            enemySounds.PlaySound(1, AudioSourceType.Weapons);
            Destroy(gameObject);
        }
    }

    public override void Move(Transform target, float movementSpeed)
    {
        Vector3 moveDirection = (target.position - transform.position).normalized;
        moveDirection = moveDirection.normalized * movementSpeed;
        moveDirection.y = 0;
        transform.rotation = Quaternion.LookRotation(moveDirection);
        rb.velocity = new(moveDirection.x, rb.velocity.y, moveDirection.z);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Handles.color = new Color(0, 0, 1, 0.2f);
        Handles.DrawSolidDisc(transform.position, Vector3.up, explodeRange);
        Handles.color = new Color(1, 0, 0, 0.1f);
        Handles.DrawSolidDisc(transform.position, Vector3.up, dashRange);
    }
#endif
}