using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotater : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] float rotationSpeed;
    [SerializeField] bool controllerEnabled;
    private Vector3 inputVector;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
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
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.y));
        Vector3 direction = mousePosition - transform.position;
        direction.y = 0;
        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }
    }

}
