using UnityEngine;

public class PlayerNetworkComponent: NetworkComponent
{
    Vector3 targetPosition;
   public void SetTargetPosition(Vector3 position)
   {
      targetPosition = position;
   }


    private void Update()
    {
        if(targetPosition == Vector3.zero) return;
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 10);
    }
}
