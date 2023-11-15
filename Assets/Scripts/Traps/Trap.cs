using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField] protected float trapDelay = 0.5f;
    protected int playerCount;
    protected float timer;

    protected void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player _))
        {
            playerCount++;
        }
    }

    protected void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Player _))
        {
            playerCount--;
            if (playerCount <= 0)
            {
                playerCount = 0;
            }
        }
    }
}