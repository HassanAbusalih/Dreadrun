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

    public void Initialize(float damage, float speed, float range, int layer, List<IProjectileEffect> effects)
    {
        rb = GetComponent<Rigidbody>();
        this.damage = damage;
        this.speed = speed;
        this.range = range;
        gameObject.layer = layer;
        this.effects = new List<IProjectileEffect>(effects);
        lastPos = transform.position;
    }

    public void Initialize(float damage, float speed, float range, int layer)
    {
        rb = GetComponent<Rigidbody>();
        this.damage = damage;
        this.speed = speed;
        this.range = range;
        gameObject.layer = layer;
        lastPos = transform.position;
    }

    public void FixedUpdate()
    {
        rb.velocity = 100 * speed * Time.deltaTime * transform.forward;
        distanceTravelled += Vector3.Distance(transform.position, lastPos);
        lastPos = transform.position;
        if (distanceTravelled > range) { Destroy(gameObject); }
    }

    private void OnCollisionEnter(Collision collision)
    {
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
        else
        {
            foreach (var effect in effects)
            {
                if (effect.ApplyOnCollision)
                {
                    effect.ApplyEffect(transform, damage, new List<IProjectileEffect>(effects));
                }
            }
        }
        if (impactVFX != null) { Instantiate(impactVFX, transform.position, Quaternion.identity); }
        Destroy(gameObject);
    }
}