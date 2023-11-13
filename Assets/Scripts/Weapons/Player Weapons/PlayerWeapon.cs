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
                _weaponCollider[i].isTrigger = true;
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
        UpdateWeaponEffects();
        weaponPickedUpOrDropped?.Invoke(this);
        _iD = weaponID;
        if (soundSO != null) soundSO.PlaySound(0, AudioSourceType.Weapons);
        if (weaponEquipText != null) weaponEquipText.SetActive(false);
    }

    public void DropWeapon()
    {
        transform.SetParent(null);
        effects.Clear();
        equipped = false;
        weaponPickedUpOrDropped?.Invoke(null);
        if (soundSO != null) soundSO.PlaySound(1, AudioSourceType.Weapons);
        if (weaponEquipText != null) weaponEquipText.SetActive(true);
    }

    public virtual void UpdateWeaponEffects()
    {
        effects.Clear();
        effects.AddRange(GetComponentsInParent<IProjectileEffect>());
    }
}