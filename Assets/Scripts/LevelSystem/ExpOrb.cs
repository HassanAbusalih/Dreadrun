using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpOrb : MonoBehaviour
{
    [SerializeField] int expAmount;
    public LayerMask layerMask;
    public ExperienceManager expmanager;

    private void Start()
    {
        expmanager = FindObjectOfType<ExperienceManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((layerMask.value & (1 << other.transform.gameObject.layer)) > 0)
        {
            expmanager.AddExperience(expAmount);
            Destroy(this.gameObject);
        }
    }
}
