using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotater : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] float rotationSpeed;
    [SerializeField] bool controllerEnabled;
    private float currentRotation = 0f;

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
        float joystickInputX = Input.GetAxis("RightStickHorizontal");
        float rotationAngle = joystickInputX * rotationSpeed;
        currentRotation += rotationAngle;
        transform.rotation = Quaternion.Euler(0, currentRotation, 0);
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
