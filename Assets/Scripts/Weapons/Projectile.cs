using UnityEngine;

[RequireComponent (typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    float damage;
    float speed;
    float range;
    Rigidbody rb;
    Vector3 initialPos;

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
        rb.velocity = transform.forward * speed * 1000 * Time.deltaTime;
        if (Vector3.Distance(transform.position, initialPos) > range) { Destroy(gameObject); }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.TryGetComponent(out IDamagable damagable)) { damagable.TakeDamage(damage); }
        Destroy(gameObject);
    }
}
