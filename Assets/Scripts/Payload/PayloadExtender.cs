using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PayloadExtender : MonoBehaviour
{
    [SerializeField] float extendedPlayerRange;
    
    float originalPlayerRange;

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Payload>() != null)
        {
            Payload payload = other.GetComponent<Payload>();
            originalPlayerRange = originalPlayerRange==0 ? payload.PlayerRange:originalPlayerRange;
            payload.PlayerRange = extendedPlayerRange;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Payload>() != null)
        {
            Payload payload = other.GetComponent<Payload>();
            payload.PlayerRange = originalPlayerRange!=0 ? originalPlayerRange : payload.PlayerRange;
            originalPlayerRange = 0;
        }
    }

 
}
