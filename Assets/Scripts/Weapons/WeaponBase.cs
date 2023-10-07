using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    protected float timeSinceLastShot;
    [SerializeField] protected float fireRate = 1;
    [SerializeField] protected int damagePerShot = 1;
    [SerializeField] protected float projectileRange = 10;
    [SerializeField] protected float projectileSpeed = 1;
    [SerializeField] protected GameObject projectilePrefab;
    [SerializeField] protected AudioSource audioSource;

    public float FireRate { get => fireRate;}
    public float ProjectileRange { get => projectileRange;}

    public abstract void Attack();
}
