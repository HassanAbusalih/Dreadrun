using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;
using UnityEngine.UI;
using Cinemachine;
using JetBrains.Annotations;

public abstract class PlayerWeapon : WeaponBase
{
    protected List<IProjectileEffect> effects = new();
    [SerializeField] protected Transform BulletSpawnPoint;

    [Header("Weapon Equip Settings")]
    [SerializeField] protected int weaponID;
    [SerializeField] Vector3 weaponOffset;
    [SerializeField] Vector3 weaponRotationOffset;
    [SerializeField] GameObject weaponEquipText;
    public bool pickedUp {get; private set;}
    protected bool equipped;

    [field: SerializeField] public Sprite weaponIcon { get; private set; }
    [field: SerializeField] public string weaponDescription { get; private set; }
    public static Action<PlayerWeapon> WeaponPickedUpOrDropped;
    Collider[] weaponColliders;
    protected CinemachineImpulseSource impulseSource;


    protected virtual void Start()
    {

        weaponColliders = GetComponents<Collider>();
        TryGetComponent(out impulseSource);
        for (int i = 0; i < weaponColliders.Length; i++)
        {
            if (weaponColliders[i].isTrigger == false)
            {
                weaponColliders[i].isTrigger = true;
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
        if (soundSO != null && !pickedUp)
        {
            soundSO.PlaySound(0, AudioSourceType.Weapons);

        }
        pickedUp = true;
        WeaponPickedUpOrDropped?.Invoke(this);
        EnableWeaponColliders(false);
        _iD = weaponID;
      
      
        if (weaponEquipText != null) weaponEquipText.SetActive(false);
    }

    public void DropWeapon()
    {
        transform.SetParent(null);
        effects.Clear();
        equipped = false;
        EnableWeaponColliders(true);
        if (soundSO != null && pickedUp)
        {
            soundSO.PlaySound(1, AudioSourceType.Weapons);

        }
        pickedUp = false;
        WeaponPickedUpOrDropped?.Invoke(this);
      
        if (weaponEquipText != null) weaponEquipText.SetActive(true);
    }

    public virtual void UpdateWeaponEffects()
    {
        effects.Clear();
        effects.AddRange(GetComponentsInParent<IProjectileEffect>());
    }

    void EnableWeaponColliders(bool enabled)
    {
        if(weaponColliders == null) return;
        for (int i = 0; i<= weaponColliders.Length-1; i++)
               {
            weaponColliders[i].enabled = enabled;
        }
    }
}