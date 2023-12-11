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

    private void OnTriggerEnter(Collider other)
    {
        bool canCollectExpOrb = ExperienceManager.CollectExp && other.TryGetComponent(out Player player);

        if (canCollectExpOrb)
        {
            OnExpOrbCollected?.Invoke(expAmount);
            if (expOrbPickUpSFX != null)
            {
                expOrbPickUpSFX.PlaySound(0, AudioSourceType.Player);
            }
            Destroy(gameObject);
        }
        else if(denySound!=null) denySound.PlaySound(0, AudioSourceType.Player);
       
    }
}
