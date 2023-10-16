using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableWeapon : MonoBehaviour, IWeaponPickable
{
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

        _iD = weaponID;
        if (pickUpSoundAudioSource != null) pickUpSoundAudioSource.Play();
    }
}
