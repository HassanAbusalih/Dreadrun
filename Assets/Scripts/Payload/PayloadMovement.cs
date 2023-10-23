using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PayloadMovement : MonoBehaviour
{
    [Header("Path Follow Settings")]
    [SerializeField] PayloadPath currentPath;
    [SerializeField] float wayPointSize;
    [SerializeField] float rotationSpeed;
    int currentNodeID = 0;

    [Header("Reverse Settings")]
    [SerializeField] float reverseCountDownTime;
    float reverseTimer;

    [Header("Object Detection Settings")]
    [SerializeField] float payloadRange;
    [SerializeField] LayerMask enemyLayer;
    [SerializeField] GameObject rangeIndicator;
    Player[] playersInGame;
    int playersOnPayload;
    bool enemyInRange;

    [Header("Checkpoint Settings")]
    PayloadCheckpointSystem checkpointSystem;
    int lastCheckpointNodeID = 0;

    [Header("Payload Stats")]
    PayloadStats payloadStats;
    float movementSpeed;

    private void Start()
    {
        payloadStats = gameObject.GetComponent<PayloadStats>();
        checkpointSystem = gameObject.GetComponent<PayloadCheckpointSystem>();
        reverseTimer = reverseCountDownTime;
        currentPath = GameObject.FindGameObjectWithTag("PayloadPath").GetComponent<PayloadPath>();
        playersInGame = FindObjectsOfType<Player>();
    }

    private void Update()
    {
        rangeIndicator.transform.localScale = new Vector3(payloadRange, .1f, payloadRange);

        StartCoroutine(ObjectsInRangeCheck());

        if (playersOnPayload > 0 && currentNodeID < currentPath.pathNodes.Count && !checkpointSystem.onCheckpoint)
        {
            reverseTimer = reverseCountDownTime;

            if (!enemyInRange)
            {
                FollowPath(movementSpeed);
            }
        }
        else if (!checkpointSystem.onCheckpoint)
        {
            FollowPath(-payloadStats.reverseSpeed);
        }
    }

    IEnumerator ObjectsInRangeCheck()
    {
        Collider[] enemiesInRange = Physics.OverlapSphere(transform.position, payloadRange, enemyLayer);

        if (enemiesInRange.Length > 0)
        {
            enemyInRange = true;
        }
        else
        {
            enemyInRange = false;
        }

        playersOnPayload = 0;

        foreach (Player player in playersInGame)
        {
            if (Vector3.Distance(transform.position, player.gameObject.transform.position) < payloadRange)
            {
                playersOnPayload++;
            }
        }

        if (playersOnPayload == 1)
        {
            movementSpeed = payloadStats.onePlayerSpeed;
        }
        else if (playersOnPayload == 2)
        {
            movementSpeed = payloadStats.twoPlayerSpeed;
        }
        else if (playersOnPayload == 3)
        {
            movementSpeed = payloadStats.threePlayerSpeed;
        }

        yield return new WaitForSeconds(.001f);
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
                if (currentPath.pathNodes[currentNodeID - 1].gameObject.CompareTag("Checkpoint") && currentNodeID > lastCheckpointNodeID)
                {
                    lastCheckpointNodeID = currentNodeID;
                    StartCoroutine(checkpointSystem.ActivateCheckpoint());
                }
            }
        }
        else if (speed < 0)
        {
            reverseTimer -= Time.deltaTime;

            if (reverseTimer <= 0 && currentNodeID > 0)
            {
                movementSpeed = speed;

                float node_Distance = Vector3.Distance(currentPath.pathNodes[currentNodeID - 1].position, transform.position);
                transform.position = Vector3.MoveTowards(transform.position, currentPath.pathNodes[currentNodeID - 1].position, Time.deltaTime * -speed);

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
