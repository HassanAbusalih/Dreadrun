using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerWeapon : WeaponBase
{
    protected bool equipped;
    protected List<IProjectileEffect> effects = new();
    [Header("Weapon Equip Settings")]
    [SerializeField] int weaponID;
    [SerializeField] Vector3 weaponOffset;
    [SerializeField] Vector3 weaponRotationOffset;
    [SerializeField] AudioSource pickUpSoundAudioSource;

    public void PickUpWeapon(Transform _weaponEquipPosition, ref int _iD)
    {
        transform.position = _weaponEquipPosition.position + weaponOffset;
        transform.SetParent(_weaponEquipPosition);
        Vector3 _weaponRotation = _weaponEquipPosition.rotation.eulerAngles + weaponRotationOffset;
        transform.rotation = Quaternion.Euler(_weaponRotation);
        UpdateWeaponEffects();
        _iD = weaponID;
        if (pickUpSoundAudioSource != null) pickUpSoundAudioSource.Play();
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