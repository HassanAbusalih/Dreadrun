using System.Collections;
using System.Collections.Generic;
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

    [Header("Checkpoint Settings")]
    private PayloadCheckpointSystem checkpointSystem;
    private int lastCheckpointNodeID = 0;

    [Header("Payload Stats")]
    private PayloadStats payloadStats;
    private float movementSpeed;

    private void Start()
    {
        payloadStats = gameObject.GetComponent<PayloadStats>();
        checkpointSystem = gameObject.GetComponent<PayloadCheckpointSystem>();
        reverseTimer = reverseCountDownTime;
        currentPath = GameObject.FindGameObjectWithTag("PayloadPath").GetComponent<PayloadPath>();
    }

    private void Update()
    {
        StartCoroutine(PlayerCheck());

        if (playersOnPayloadCount > 0 && currentNodeID < currentPath.pathNodes.Count && checkpointSystem.onCheckpoint == false)
        {
            reverseTimer = reverseCountDownTime;

            FollowPath(movementSpeed * 1);
        }
        else if (checkpointSystem.onCheckpoint == false)
        {
            FollowPath(movementSpeed * -1);
        }
    }

    IEnumerator PlayerCheck()
    {
        Collider[] playersOnPayload = Physics.OverlapSphere(transform.position, payloadRange, playerLayer);

        playersOnPayloadCount = playersOnPayload.Length;

        if (playersOnPayloadCount == 1)
        {
            movementSpeed = payloadStats.onePlayerSpeed;
        }
        else if (playersOnPayloadCount == 2)
        {
            movementSpeed = payloadStats.twoPlayerSpeed;
        }
        else if (playersOnPayloadCount == 3)
        {
            movementSpeed = payloadStats.threePlayerSpeed;
        }

        yield return new WaitForSeconds(.1f);
    }

    private void FollowPath(float speed)
    {
        if (speed > 0)
        {

            float node_Distance = Vector3.Distance(currentPath.pathNodes[currentNodeID].position, transform.position);
            transform.position = Vector3.MoveTowards(transform.position, currentPath.pathNodes[currentNodeID].position, Time.deltaTime * speed);

            Quaternion target_Rotation = Quaternion.LookRotation(currentPath.pathNodes[currentNodeID].position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, target_Rotation, Time.deltaTime * rotationSpeed);

            if (node_Distance <= wayPointSize)
            {
                currentNodeID++;
                if (currentPath.pathNodes[currentNodeID].gameObject.CompareTag("Checkpoint") && currentNodeID > lastCheckpointNodeID)
                {
                    lastCheckpointNodeID = currentNodeID;
                    //checkpoint.ActivateCheckpoint();
                }
            }
        }
        else if (speed < 0) 
        {
            reverseTimer -= Time.deltaTime;

            if (reverseTimer <= 0 && currentNodeID > 0)
            {
                movementSpeed = speed * -1;

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
