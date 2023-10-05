using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PayloadFollowPath : MonoBehaviour
{
    public PayloadPath currentPath;
    public int currentNodeID = 0;
    public float Speed = 5f;
    public float RotationSpeed = 5f;
    public float WayPointSize = 1f;

    public GameObject player;
    public float payloadMoveRange;


    private void Start()
    {
        currentPath = GameObject.FindGameObjectWithTag("PayloadPath").GetComponent<PayloadPath>();
        player = GameObject.FindGameObjectWithTag("Player");
    }
    private void Update()
    {
        if (Vector3.Distance(player.transform.position, transform.position) < payloadMoveRange && currentNodeID < currentPath.pathNodes.Count)
        {
            float node_Distance = Vector3.Distance(currentPath.pathNodes[currentNodeID].position, transform.position);
            transform.position = Vector3.MoveTowards(transform.position, currentPath.pathNodes[currentNodeID].position, Time.deltaTime * Speed);

            Quaternion target_Rotation = Quaternion.LookRotation(currentPath.pathNodes[currentNodeID].position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, target_Rotation, Time.deltaTime * RotationSpeed);

            if (node_Distance <= WayPointSize)
            {
                currentNodeID++;
            }
        }
    }
}
