using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ExpOrb : MonoBehaviour
{
    [SerializeField] int expAmount;
    public LayerMask layerMask;
    public ExperienceManager expmanager;
    [SerializeField] SoundSO expOrbPickUpSFX;

    private void Start()
    {
        expmanager = FindObjectOfType<ExperienceManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            expmanager.AddExperience(expAmount);
            Destroy(this.gameObject);
        }
    }
    private void OnDestroy()
    {
        expOrbPickUpSFX.Play();
    }
}
