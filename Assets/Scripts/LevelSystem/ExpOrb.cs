using System;
using UnityEngine;

public class ExpOrb : MonoBehaviour
{
    public static event Action<int> OnExpOrbCollected;
    [SerializeField] int expAmount;
    public int ExpAmount => expAmount;
    [SerializeField] LayerMask layerMask;
    [SerializeField] SoundSO expOrbPickUpSFX;

    private void OnTriggerEnter(Collider other)
    {
        if (ExperienceManager.CollectExp && other.TryGetComponent(out Player player))
        {
            OnExpOrbCollected?.Invoke(expAmount);
            if (expOrbPickUpSFX != null)
            {
                expOrbPickUpSFX.PlaySound(0, AudioSourceType.Player);
            }
            Destroy(gameObject);
        }
    }
}
