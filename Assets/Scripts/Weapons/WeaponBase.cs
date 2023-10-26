using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    protected float timeSinceLastShot;
    [Header("Base Weapon Properties")]
    [SerializeField] protected float fireRate = 1;
    [SerializeField] protected float damageModifier = 1;
    [SerializeField] protected float projectileRange = 10;
    [SerializeField] protected float projectileSpeed = 1;
    [SerializeField] protected float spreadAngle = 30;
    [SerializeField] protected GameObject projectilePrefab;
    [SerializeField] protected AudioSource audioSource;
    public float FireRate { get => fireRate; set => fireRate = value; }
    public float ProjectileRange { get => projectileRange; set => projectileRange = value; }
    public float ProjectileSpeed { get => projectileSpeed; set => projectileSpeed = value; }
    public float DamageModifier { get => damageModifier; set => damageModifier = value; }
    public float SpreadAngle { get => spreadAngle; set => spreadAngle = value; }

    public abstract void Attack();
}