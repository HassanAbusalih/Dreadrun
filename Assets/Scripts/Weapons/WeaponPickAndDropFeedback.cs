using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickAndDropFeedback : MonoBehaviour
{
    [SerializeField] Renderer weaponRenderer;
    [SerializeField] Light rarityLight;
     RotationAnimator rotationAnimator => GetComponentInChildren<RotationAnimator>();

    private void OnEnable() => PlayerWeapon.WeaponPickedUpOrDropped += DropAndPickUpFeedback;


    private void OnDisable() => PlayerWeapon.WeaponPickedUpOrDropped -= DropAndPickUpFeedback;


    private void Start()
    {
        rarityLight = GetComponentInChildren<Light>();
    }


    void DropAndPickUpFeedback(PlayerWeapon weapon)
    {
        if (weapon == null) return;
        TryGetComponent(out PlayerWeapon weaponInCurrentObject);

        if (weapon != GetComponent<PlayerWeapon>()) return;

        if (rarityLight != null)
            rarityLight.enabled = !weapon.pickedUp;
        if(rotationAnimator != null)
            rotationAnimator.Disabled = weapon.pickedUp;
    }

}
