using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFocus : MonoBehaviour
{
    public Transform player;
    public float smoothSpeed = 0.125f;
    public float rotationSpeed;
    [SerializeField] float backwardOffset = 5f;
    [SerializeField] float upwardOffset = 2f;
    [SerializeField] float xRotation = 0f;
    Vector3 forwardDirection;
    Vector3 offset;

    private void Start()
    {
        player = FindObjectOfType<Player>().transform;
        forwardDirection = player.transform.forward;
        Quaternion lookDirection = Quaternion.LookRotation(forwardDirection, Vector3.up);
        transform.rotation = lookDirection;
        transform.rotation = Quaternion.Euler(xRotation, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
    }

    void FixedUpdate()
    {
        Vector3 backOffset = forwardDirection * backwardOffset;
        offset = new Vector3(-backOffset.x, -backOffset.y + upwardOffset, -backOffset.z);
        Vector3 desiredPosition = player.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }

    public void UpdateRotation(Transform newForward)
    {
        forwardDirection = newForward.forward;
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, newForward.eulerAngles.y, transform.eulerAngles.z);
    }

    private void Update()
    {
        MouseRotation();
    }
    void MouseRotation()
    {
        Vector3 mouseScreenPosition = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mouseScreenPosition);
        Plane groundPlane = new Plane(Vector3.up, transform.position);

        float rayDistance;
        if (groundPlane.Raycast(ray, out rayDistance))
        {
            Vector3 point = ray.GetPoint(rayDistance);
            Vector3 direction = (point - transform.position).normalized;
            direction.y = 0;
            Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
