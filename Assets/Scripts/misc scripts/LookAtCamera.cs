using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private Camera mainCamera;
    Quaternion originalRotation;

    private void Start()
    {
        mainCamera = Camera.main;
        originalRotation = transform.localRotation;
    }

    private void Update()
    {
        if (mainCamera != null)
        {
            Vector3 _lookDirection = (mainCamera.transform.position - transform.position).normalized;
            _lookDirection.y = 0; // this for some reason allows the y to rotate, it makes no sense but it works
            Quaternion _lookAtRotation = Quaternion.LookRotation(-_lookDirection);
            if (transform.parent != null)
            {
                _lookAtRotation = Quaternion.Inverse(transform.parent.rotation) * _lookAtRotation;
            }
            transform.localRotation = _lookAtRotation * originalRotation; 
        }
    }
}
