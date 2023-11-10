using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;
using UnityEngine.UI;

public abstract class PlayerWeapon : WeaponBase
{
    protected List<IProjectileEffect> effects = new();
    [SerializeField] protected Transform BulletSpawnPoint;

    [Header("Weapon Equip Settings")]
    [SerializeField] protected int weaponID;
    [SerializeField] Vector3 weaponOffset;
    [SerializeField] Vector3 weaponRotationOffset;
    [SerializeField] GameObject weaponEquipText;
    protected bool equipped;
    [SerializeField] Collider weaponCollider;


    [field: SerializeField] public Sprite weaponIcon { get; private set; }
    [field: SerializeField] public string weaponDescription { get; private set; }
    public static Action<PlayerWeapon> weaponPickedUpOrDropped;


    protected virtual void Start()
    {

        Collider[] _weaponCollider = GetComponents<Collider>();
        for (int i = 0; i < _weaponCollider.Length; i++)
        {
            if (_weaponCollider[i].isTrigger == false)
            {
                weaponCollider = _weaponCollider[i];
            }
        }

    }

    public void PickUpWeapon(Transform _weaponEquipPosition, ref int _iD)
    {
        transform.position = _weaponEquipPosition.position + weaponOffset;
        transform.SetParent(_weaponEquipPosition);
        Vector3 _weaponRotation = _weaponEquipPosition.rotation.eulerAngles + weaponRotationOffset;
        transform.rotation = Quaternion.Euler(_weaponRotation);
        equipped = true;
        weaponCollider.isTrigger = true;

        soundSO.PlaySound(ref audioSource, 0, this.gameObject);

        UpdateWeaponEffects();
        weaponPickedUpOrDropped?.Invoke(this);
        _iD = weaponID;
        if (weaponEquipText != null) weaponEquipText.SetActive(false);
    }

    public void DropWeapon()
    {
        transform.SetParent(null);
        effects.Clear();
        equipped = false;
        weaponCollider.isTrigger = false;
        soundSO.PlaySound(ref audioSource, 1, this.gameObject);
        weaponPickedUpOrDropped?.Invoke(null);
        if (weaponEquipText != null) weaponEquipText.SetActive(true);
    }

    public virtual void UpdateWeaponEffects()
    {
        effects.Clear();
        effects.AddRange(GetComponentsInParent<IProjectileEffect>());
    }
}