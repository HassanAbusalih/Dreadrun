using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    float damage;
    float speed;
    float range;
    Rigidbody rb;
    Vector3 lastPos;
    float distanceTravelled = 0;
    List<IProjectileEffect> effects;
    [SerializeField] int layerToIgnore;
    [SerializeField] GameObject impactVFX;
    [SerializeField][Range(0, 1)] float targetScalePercent = 0.5f;
    Transform transformToScale;
    Vector3 originalScale;
    SphereCollider sphereCollider;
    float colliderRadius;

    public void Initialize(float damage, float speed, float range, int layer, List<IProjectileEffect> effects)
    {
        rb = GetComponent<Rigidbody>();
        this.damage = damage;
        this.speed = speed;
        this.range = range;
        gameObject.layer = layer;
        this.effects = new List<IProjectileEffect>(effects);
        lastPos = transform.position;
        transformToScale = GetComponentInChildren<SpriteRenderer>().transform;
        originalScale = transformToScale.localScale;
    }

    public void Initialize(float damage, float speed, float range, int layer)
    {
        rb = GetComponent<Rigidbody>();
        this.damage = damage;
        this.speed = speed;
        this.range = range;
        gameObject.layer = layer;
        transformToScale = transform;
        originalScale = transformToScale.localScale;
        sphereCollider = GetComponent<SphereCollider>();
        colliderRadius = sphereCollider.radius;
        lastPos = transform.position;
    }

    public void FixedUpdate()
    {
        rb.velocity = 100 * speed * Time.deltaTime * transform.forward;
        distanceTravelled += Vector3.Distance(transform.position, lastPos);
        lastPos = transform.position;
        transformToScale.localScale = Vector3.Lerp(originalScale, originalScale * targetScalePercent, distanceTravelled / range);
        if (sphereCollider != null)
        {
            float scaleFactor = originalScale.x / transform.localScale.x;
            sphereCollider.radius = colliderRadius * scaleFactor;
        }
        if (distanceTravelled > range)
        {
            GameObject vfx = null;
            if (impactVFX != null) {vfx = Instantiate(impactVFX, transform.position, Quaternion.identity); }
            Destroy(vfx, 2f);
            gameObject.SetActive(false);
            distanceTravelled = 0;
            transformToScale.localScale = Vector3.one * 4;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject vfx = null;
        if (collision.transform.TryGetComponent(out IDamagable damagable))
        {
            if (collision.gameObject.layer == layerToIgnore) { return; }
            damagable.TakeDamage(damage);
            if (effects != null && !damagable.gameObject.TryGetComponent(out PayloadStats payload))
            {
                foreach (IProjectileEffect effect in effects)
                {
                    effect.ApplyEffect(damagable, damage, new List<IProjectileEffect>(effects));
                }
            }
        }
        else if (effects != null && effects.Count > 0)
        {
            foreach (var effect in effects)
            {
                if (effect.ApplyOnCollision)
                {
                    effect.ApplyEffect(transform, damage, new List<IProjectileEffect>(effects));
                }
            }
        }
        if (impactVFX != null) { vfx = Instantiate(impactVFX, transform.position, Quaternion.identity);
        }

        Destroy(vfx, 2f);
        gameObject.SetActive(false);
    }      
}