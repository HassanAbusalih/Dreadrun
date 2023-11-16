using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class PlayerRotater : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] float rotationSpeed;
    [SerializeField] bool controllerEnabled;
    private Vector3 inputVector;
    float mouseHorizontal;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (controllerEnabled)
        {
            ControllerRotation();
        }
        else
        {
            MouseRotation();
        }
    }

    void ControllerRotation()
    {
        float rightStickX = Input.GetAxis("RightStickX");
        float rightStickY = Input.GetAxis("RightStickY");

        // Calculate the input vector
        Vector3 inputVector = new Vector3(rightStickX, rightStickY, 0);

        // Rotate the player based on input
        if (inputVector != Vector3.zero)
        {
            float targetAngle = Mathf.Atan2(inputVector.x, inputVector.y) * Mathf.Rad2Deg;
            Vector3 currentEulerAngles = transform.eulerAngles;
            currentEulerAngles.y = Mathf.MoveTowardsAngle(currentEulerAngles.y, targetAngle, rotationSpeed * Time.deltaTime);
            transform.eulerAngles = currentEulerAngles;
        }
    }

    void MouseRotation()
    {

        mouseHorizontal += Input.GetAxis("Mouse X") * Time.deltaTime;
        Vector3 mouseDirection = new Vector3(0, mouseHorizontal * rotationSpeed, 0);
        transform.localRotation = Quaternion.Euler(new Vector3(transform.localRotation.x,mouseDirection.y,transform.rotation.z));


    }

}
