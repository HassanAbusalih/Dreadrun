using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtObject : MonoBehaviour
{
    [SerializeField] Transform objectToLookAt;
    public bool invertLookAt;
    Vector3 startRotation;
    [SerializeField]bool LookAtCamFollow;

    private void Start()
    {
        if(invertLookAt)
        startRotation = -transform.rotation.eulerAngles;
        else startRotation = transform.rotation.eulerAngles;
        if(LookAtCamFollow) objectToLookAt = FindObjectOfType<CameraFocus>().transform;
    }

    private void Update()
    {
        Vector3 direction = (objectToLookAt.position - transform.position).normalized;
        direction.y = 0; // this for some reason allows the y to rotate, it makes no sense but it works
        Quaternion _lookAtRotation = Quaternion.LookRotation(direction);
        


        if (invertLookAt)
        {
           transform.rotation = Quaternion.LookRotation(-direction) * Quaternion.Euler(startRotation);
        }
        else
        {
            transform.rotation =  Quaternion.LookRotation(direction) * Quaternion.Euler(startRotation);
        }
    }
}
