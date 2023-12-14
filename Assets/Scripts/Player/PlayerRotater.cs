using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class PlayerRotater : MonoBehaviour
{
    [SerializeField] float rotationSpeed;
    [SerializeField] bool controllerEnabled;
    [SerializeField] float deadZone = 50f;
    float mouseHorizontal;
    [SerializeField]float smoothingFactor;

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
        float horizontalInput = Input.GetAxis("RightStickX");
        float verticalInput = Input.GetAxis("RightStickY");
        Vector3 inputDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        if (inputDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(inputDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed);
        }
    }

    void MouseRotation()
    {
        Vector3 mouseScreenPosition = Input.mousePosition;

        // Calculate the distance from the mouse position to the center of the screen
        float mouseDistanceFromCenter = Vector3.Distance(mouseScreenPosition, new Vector3(Screen.width / 2, Screen.height / 2));

        if (mouseDistanceFromCenter > deadZone)
        {
            // Only rotate if the mouse is beyond the offset threshold from the center

            // Create a ray from the camera to the mouse position
            Ray ray = Camera.main.ScreenPointToRay(mouseScreenPosition);

            mouseHorizontal += Input.GetAxis("Mouse X") * rotationSpeed; // Remove Time.deltaTime here

            // Smooth the rotation using Mathf.Lerp or another smoothing method if needed
            float smoothMouseHorizontal = Mathf.Lerp(mouseHorizontal, 0f, smoothingFactor);

            // Apply the rotation around the y-axis
            transform.Rotate(Vector3.up, smoothMouseHorizontal);

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

                // Smoothly rotate towards the target direction
                transform.forward = Vector3.Slerp(transform.forward, direction, smoothingFactor * Time.deltaTime);
            }
        }
    }

}