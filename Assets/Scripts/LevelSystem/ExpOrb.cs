using System;
using UnityEngine;
using TMPro;

public class ExpOrb : MonoBehaviour
{
    public static event Action<int> OnExpOrbCollected;
    [SerializeField] int expAmount;
    public int ExpAmount => expAmount;
    [SerializeField] LayerMask layerMask;
    [SerializeField] SoundSO expOrbPickUpSFX;
    [SerializeField] SoundSO denySound;
    [SerializeField] GameObject vfx;
    [SerializeField] GameObject expTextObject;
    [SerializeField] Color expTextColor;

    private void OnTriggerEnter(Collider other)
    {

        bool isPlayerThere = other.TryGetComponent(out Player player);


        if (ExperienceManager.CollectExp && isPlayerThere)
        {
            OnExpOrbCollected?.Invoke(expAmount);

            Quaternion camRotation = FindObjectOfType<CameraFocus>().transform.localRotation;
            Quaternion expTextRotation = Quaternion.Euler(expTextObject.transform.eulerAngles.x, camRotation.eulerAngles.y, camRotation.eulerAngles.z);
            GameObject expText = Instantiate(expTextObject, transform.position + (Vector3.up*5),expTextRotation);
            expText.GetComponent<TextMeshPro>().text = $"+{expAmount}XP";
            expText.GetComponent<TextMeshPro>().color = expTextColor;

            if (expOrbPickUpSFX != null)
            {
                expOrbPickUpSFX.PlaySound(0, AudioSourceType.Player);
                if(vfx != null)
                {
                    GameObject vfxObj = Instantiate(vfx, transform.position, Quaternion.identity);
                    Destroy(vfxObj, 1.25f);
                }
               
            }
            Destroy(gameObject);
            Destroy(expText, 3f);
        }
        else if(!ExperienceManager.CollectExp && isPlayerThere) denySound.PlaySound(0, AudioSourceType.Player);
       
    }
}
