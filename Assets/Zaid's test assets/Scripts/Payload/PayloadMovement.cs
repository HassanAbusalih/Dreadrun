using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class PayloadMovement : MonoBehaviour
{
    [Header("Path Follow Settings")]
    public PayloadPath currentPath;
    public float wayPointSize;
    public float rotationSpeed;
    private int currentNodeID = 0;

    [Header("Reverse Settings")]
    public float reverseCountDownTime;
    private float reverseTimer;

    [Header("Player Detection Settings")]
    public float payloadRange;
    public LayerMask playerLayer;
    private int playersOnPayloadCount;

    #region PayloadStats
    private PayloadStats payloadStats;
    private float speed;
    #endregion

    private void Start()
    {
        payloadStats = gameObject.GetComponent<PayloadStats>();
        reverseTimer = reverseCountDownTime;
        currentPath = GameObject.FindGameObjectWithTag("PayloadPath").GetComponent<PayloadPath>();
    }
    private void Update()
    {
        Collider[] playersOnPayload = Physics.OverlapSphere(transform.position, payloadRange, playerLayer);
        playersOnPayloadCount = playersOnPayload.Length;

        if (playersOnPayloadCount > 0 && currentNodeID < currentPath.pathNodes.Count)
        {
            reverseTimer = reverseCountDownTime;

            if (playersOnPayloadCount == 1)
            {
                speed = payloadStats.onePlayerSpeed;
            }
            else if (playersOnPayloadCount == 2)
            {
                speed = payloadStats.twoPlayerSpeed;
            }
            else if (playersOnPayloadCount == 3)
            {
                speed = payloadStats.threePlayerSpeed;
            }

            float node_Distance = Vector3.Distance(currentPath.pathNodes[currentNodeID].position, transform.position);
            transform.position = Vector3.MoveTowards(transform.position, currentPath.pathNodes[currentNodeID].position, Time.deltaTime * speed);

            Quaternion target_Rotation = Quaternion.LookRotation(currentPath.pathNodes[currentNodeID].position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, target_Rotation, Time.deltaTime * rotationSpeed);

            if (node_Distance <= wayPointSize)
            {
                currentNodeID++;
            }
        }
        else
        {
            reverseTimer -= Time.deltaTime;

            if (reverseTimer <= 0 && currentNodeID > 0)
            {
                speed = payloadStats.reverseSpeed;

                float node_Distance = Vector3.Distance(currentPath.pathNodes[currentNodeID - 1].position, transform.position);
                transform.position = Vector3.MoveTowards(transform.position, currentPath.pathNodes[currentNodeID - 1].position, Time.deltaTime * speed);

                Quaternion target_Rotation = Quaternion.LookRotation(currentPath.pathNodes[currentNodeID].position - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, target_Rotation, Time.deltaTime * rotationSpeed);

                if (node_Distance <= wayPointSize)
                {
                    currentNodeID--;
                }
            }
        }
    }
}
