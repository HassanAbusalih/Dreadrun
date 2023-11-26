using UnityEngine;

public class PlayerNetworkComponent: NetworkComponent
{
    Vector3 targetPosition;
    Quaternion targetRotation;

   public void SetTargetPosition(Vector3 position, Quaternion rotation, float tickRate)
   {
        transform.LeanMove(position, tickRate);
        transform.LeanRotate(rotation.eulerAngles, tickRate);
   }
}
