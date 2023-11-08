using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpOrb : MonoBehaviour
{
    [SerializeField] int expAmount;
    public int  ExpAmount => expAmount;
    public LayerMask layerMask;
    public ExperienceManager expmanager;

    private void Start()
    {
        expmanager = FindObjectOfType<ExperienceManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Player player))
        {
            expmanager.AddExperience(expAmount);
            Destroy(this.gameObject);
        }
    }
}
