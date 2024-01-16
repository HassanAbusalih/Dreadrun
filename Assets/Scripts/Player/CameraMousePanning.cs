using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMousePanning : MonoBehaviour
{
    [SerializeField]Transform mainCameraTransform;
    [SerializeField] float panSpeed = 20f;
    [SerializeField] float xMaxPan;
    [SerializeField] float zMaxPan;
    float interpolatedXPan;
    float interpolatedZPan;

    [Header("Debug Info")]
    [SerializeField]Vector3 mousePosition;
    [SerializeField] float xPan;
    [SerializeField] float zPan;


    void Update()
    {
        PanCameraWithMouse();
    }

    void PanCameraWithMouse()
    {
        Vector3 mousePosition = Input.mousePosition;
        this.mousePosition = mousePosition;

        float xPercentageValue = Mathf.InverseLerp(0, Screen.width, mousePosition.x);
        float zPercentageValue = Mathf.InverseLerp(0, Screen.height, mousePosition.y);


        float currentXPan = Mathf.Lerp(-xMaxPan, xMaxPan, xPercentageValue);
        float currentZPan = Mathf.Lerp(-zMaxPan, zMaxPan, zPercentageValue);

        interpolatedXPan = Mathf.Lerp(interpolatedXPan, currentXPan, Time.deltaTime * panSpeed);
        interpolatedZPan = Mathf.Lerp(interpolatedZPan, currentZPan, Time.deltaTime * panSpeed);

        

        xPan = currentXPan;
        zPan = currentZPan;

        Vector3 newPosition = new Vector3(interpolatedXPan,0,interpolatedZPan);

        transform.localPosition = newPosition;
    }
}
