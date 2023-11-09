using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactStatus : MonoBehaviour
{
    GameObject player;
    [SerializeField] float increaseSpeed;
    float defaultSpeed;
    void Start()
    {
        player = GameObject.FindObjectOfType<Player>().gameObject;
        defaultSpeed = player.GetComponent<Player>().playerStats.speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject)
        {
            player.GetComponent<Player>().playerStats.speed += increaseSpeed;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject)
        {
            player.GetComponent<Player>().playerStats.speed = defaultSpeed;
        }
    }
}
