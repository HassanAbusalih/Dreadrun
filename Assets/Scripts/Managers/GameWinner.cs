using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameWinner : MonoBehaviour
{
    bool playerInside = false;
    bool payloadInside = false;
    private float insideTimer = 0f;
    [SerializeField] float requiredTime = 10f;


    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            playerInside = true;
        }
        if (other.TryGetComponent(out Payload payload))
        {
            payloadInside = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Player player) && playerInside)
        {
            playerInside = false;
            insideTimer = 0f;
        }
        if (other.TryGetComponent(out Payload payload))
        {
            payloadInside = false;
            insideTimer = 0f;
        }
    }

    private void Update()
    {
        if (playerInside && payloadInside)
        {
            insideTimer += Time.deltaTime;

            if (insideTimer >= requiredTime)
            {
                GameManager.Instance.Win();
            }
        }
    }
}
