using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class PlayerWeapon : WeaponBase
{
    protected bool equipped;
    protected List<IProjectileEffect> effects = new();
    [SerializeField] protected Transform BulletSpawnPoint;
    [Header("Weapon Equip Settings")]
    [SerializeField] int weaponID;
    [SerializeField] Vector3 weaponOffset;
    [SerializeField] Vector3 weaponRotationOffset;
    [SerializeField] AudioSource pickUpSoundAudioSource;
    [SerializeField] GameObject weaponEquipText;


    public void PickUpWeapon(Transform _weaponEquipPosition, ref int _iD)
    {
        transform.position = _weaponEquipPosition.position + weaponOffset;
        transform.SetParent(_weaponEquipPosition);
        Vector3 _weaponRotation = _weaponEquipPosition.rotation.eulerAngles + weaponRotationOffset;
        transform.rotation = Quaternion.Euler(_weaponRotation);
        UpdateWeaponEffects();
        _iD = weaponID;
        if (pickUpSoundAudioSource != null) pickUpSoundAudioSource.Play();
        if(weaponEquipText != null) weaponEquipText.SetActive(false);   
    }

    public void DropWeapon()
    {
        transform.SetParent(null);
        effects.Clear();
        if (weaponEquipText != null) weaponEquipText.SetActive(true);
    }


    private void LateUpdate()
    {
        if (transform.parent != null)
        {
            if (!equipped)
            {
                equipped = GetComponentInParent<Player>() != null;
            }
        }
        else
        {
            equipped = false;
        }
    }

    public virtual void UpdateWeaponEffects()
    {
        effects.Clear();
        effects.AddRange(GetComponentsInParent<IProjectileEffect>());
    }
}