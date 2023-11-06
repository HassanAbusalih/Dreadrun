using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private Camera mainCamera;
    Quaternion originalRotation;

    private void Start()
    {
        // Find the main camera in the scene
        mainCamera = Camera.main;
        originalRotation = transform.rotation;
    }

    private void Update()
    {
        if (mainCamera != null)
        {
            Vector3 lookDirection = mainCamera.transform.position - transform.position;
            lookDirection.y = 0; // this for some reason allows the y to rotate, it makes no sense but it works
            Quaternion newRotation = Quaternion.LookRotation(-lookDirection);
            Vector3 eulerAngles = newRotation.eulerAngles;
          
            newRotation.eulerAngles = eulerAngles;
            transform.rotation = newRotation * originalRotation; 
        }
    }
}
