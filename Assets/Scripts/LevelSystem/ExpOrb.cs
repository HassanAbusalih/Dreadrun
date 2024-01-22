using System;
using UnityEngine;

public class ExpOrb : MonoBehaviour
{
    public static event Action<int> OnExpOrbCollected;
    [SerializeField] int expAmount;
    public int ExpAmount => expAmount;
    [SerializeField] LayerMask layerMask;
    [SerializeField] SoundSO expOrbPickUpSFX;
    [SerializeField] SoundSO denySound;
    [SerializeField] GameObject vfx;

    private void OnTriggerEnter(Collider other)
    {

        bool isPlayerThere = other.TryGetComponent(out Player player);


        if (ExperienceManager.CollectExp && isPlayerThere)
        {
            OnExpOrbCollected?.Invoke(expAmount);
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
        }
        else if(!ExperienceManager.CollectExp && isPlayerThere) denySound.PlaySound(0, AudioSourceType.Player);
       
    }
}
