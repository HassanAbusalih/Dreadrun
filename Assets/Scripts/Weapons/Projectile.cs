using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    float damage;
    float speed;
    float range;
    Rigidbody rb;
    Vector3 initialPos;
    List<IProjectileEffect> effects;

    public void Initialize(float damage, float speed, float range, int layer, List<IProjectileEffect> effects)
    {
        rb = GetComponent<Rigidbody>();
        this.damage = damage;
        this.speed = speed;
        this.range = range;
        this.effects = effects;
        gameObject.layer = layer;
        initialPos = transform.position;
    }

    public void Initialize(float damage, float speed, float range, int layer)
    {
        rb = GetComponent<Rigidbody>();
        this.damage = damage;
        this.speed = speed;
        this.range = range;
        gameObject.layer = layer;
        initialPos = transform.position;
    }

    public void FixedUpdate()
    {
        rb.velocity = transform.forward * speed * 100 * Time.deltaTime;
        if (Vector3.Distance(transform.position, initialPos) > range) { Destroy(gameObject); }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.TryGetComponent(out IDamagable damagable)) 
        { 
            damagable.TakeDamage(damage); 
            if(effects !=null)
            {
                foreach (IProjectileEffect effect in effects)
                {
                    effect.ApplyEffect(damagable, damage, effects);
                }
            }
              
        }
        Destroy(gameObject);
    }
}