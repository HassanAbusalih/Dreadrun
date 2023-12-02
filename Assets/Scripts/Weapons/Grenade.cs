using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Grenade : MonoBehaviour
{
    [SerializeField] float height = 1;
    [SerializeField] float explosionRadius = 5;
    [SerializeField] float explosionForce = 700;
    Vector3 target;
    Vector3 start;
    Vector3 offset;
    Vector3 controlPoint;
    float damage;
    float currentTime;
    float duration;
    Rigidbody rb;
    List<IProjectileEffect> effects;
    [SerializeField] LayerMask layersToIgnore;
    [SerializeField] GameObject explosionVFX;
    [SerializeField] SoundSO explosionSFX;
    AudioSource audioSource;

    public void Initialize(Vector3 target, float speed, float damage, int layer, List<IProjectileEffect> effects)
    {
        this.damage = damage;
        this.target = target;
        gameObject.layer = layer;
        start = transform.position;
        offset = new(0, height, 0);
        controlPoint = start + (target - start) / 2 + offset;
        duration = Vector3.Distance(transform.position, target) / speed;
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        this.effects = effects;
    }

    public void Initialize(Vector3 target, float speed, float damage, int layer)
    {
        this.damage = damage;
        this.target = target;
        gameObject.layer = layer;
        start = transform.position;
        offset = new(0, height, 0);
        controlPoint = start + (target - start) / 2 + offset;
        duration = Vector3.Distance(transform.position, target) / speed;
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    private void FixedUpdate()
    {
        currentTime += Time.deltaTime;
        float t = currentTime / duration;
        Vector3 position = (1 - t) * (1 - t) * start + 2 * (1 - t) * t * controlPoint + t * t * target;
        rb.MovePosition(position);
        if (t >= 1)
        {
            Explode();
            return;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Explode();
    }

    void Explode()
    {
        Explosion.Explode(transform, damage, explosionRadius, explosionForce, layersToIgnore, effects);
        Instantiate(explosionVFX, transform.position, transform.rotation);
        if(TryGetComponent(out CinemachineImpulseSource shake))
        {
            shake.GenerateImpulse();
        }
        if (explosionSFX != null)
        {
            explosionSFX.PlaySound(0, AudioSourceType.Weapons);
        }
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 1, 0, 0.2f);
        Gizmos.DrawSphere(transform.position, explosionRadius);
    }
}