using UnityEngine;

public class PlayerNetworkComponent : NetworkComponent
{
    Vector3 targetPosition;
    Quaternion targetRotation;

    public void SetTargetPosition(Vector3 position, Quaternion rotation)
    {
        targetPosition = position;
        targetRotation = rotation;
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime);
    }
}
