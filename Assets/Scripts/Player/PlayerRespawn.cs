using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    private Player player;
    public List<GameObject> checkpoints = new List<GameObject>();
    private GameObject currentCheckpoint;

    void Start()
    {
        player = GetComponent<Player>();
        currentCheckpoint = checkpoints[0];
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Checkpoint"))
        {
            currentCheckpoint = other.gameObject;
        }

        if (other.CompareTag("HarmfulObject"))
        {
            InstaRespawnHazard hazard = other.GetComponent<InstaRespawnHazard>();
            if (hazard != null)
            {
                float damageTaken = hazard.damageAmount;
                player.TakeDamage(damageTaken);
            }
            Respawn();
        }
    }

    public void Respawn()
    {
        player.transform.position = currentCheckpoint.transform.position;
    }
}
