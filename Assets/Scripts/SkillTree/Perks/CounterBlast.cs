using System;
using UnityEngine;

public class CounterBlast : MonoBehaviour, INeedUI
{
    private float explosionRadius = 4f;
    private float explosionForce = 4000f;

    private float explosionCooldown = 5f;
    private float explosionTimer;
    private LayerMask layersToIgnore;
    private GameObject vfx;

    public event Action OnCoolDown;

    private void Start()
    {
        explosionTimer = explosionCooldown;
    }

    private void Update()
    {
        if (explosionTimer < explosionCooldown)
        {
            explosionTimer += Time.deltaTime;
        }
    }

    public void Explode(float damage)
    {
        if (explosionTimer < explosionCooldown)
        {
            return;
        }

        explosionTimer = 0;
        OnCoolDown.Invoke();

        Explosion.Explode(transform, damage, explosionRadius, explosionForce, layersToIgnore);

        Instantiate(vfx, transform.position, transform.rotation);
    }
    public void SetCounterBlast(float radius, float force, LayerMask layerMask, GameObject vfx, float cooldown)
    {
        explosionRadius = radius;
        explosionForce = force;
        layersToIgnore = layerMask;
        this.vfx = vfx;
        explosionCooldown = cooldown;
    }
}