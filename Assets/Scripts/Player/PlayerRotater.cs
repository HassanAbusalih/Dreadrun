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
        Vector3 mouseScreenPosition = Input.mousePosition;

        // Create a ray from the camera to the mouse position
        Ray ray = Camera.main.ScreenPointToRay(mouseScreenPosition);

        // Create a plane at the player's height, assuming 'up' is your ground plane normal
        Plane groundPlane = new Plane(Vector3.up, transform.position);

        float rayDistance;
        if (groundPlane.Raycast(ray, out rayDistance))
        {
            // Find the point in the world where the mouse is pointing, at the player's height
            Vector3 point = ray.GetPoint(rayDistance);

            // Calculate the direction vector from the player to this point
            Vector3 direction = (point - transform.position).normalized;

            // Adjust the direction to be in the horizontal plane
            direction.y = 0;

            // Assign this direction to the player's transform.forward
            transform.forward = direction;
        }
    }

}
