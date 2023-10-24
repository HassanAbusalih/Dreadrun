using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PayloadMovement : MonoBehaviour
{
    public bool movementEnabled = false;

    [Header("Path Follow Settings")]
    [SerializeField] PayloadPath currentPath;
    [SerializeField] float wayPointSize, movementSpeed;
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
        if (movementEnabled)
        {
            rangeIndicator.SetActive(true);
            rangeIndicator.transform.localScale = new Vector3(payloadRange / transform.localScale.x, .1f / transform.localScale.y, payloadRange / transform.localScale.z);

            StartCoroutine(ObjectsInRangeCheck());

            if (playersOnPayload > 0 && currentNodeID < currentPath.pathNodes.Count && !checkpointSystem.onCheckpoint)
            {
                reverseTimer = reverseCountDownTime;

                if (!enemyInRange)
                {
                    FollowPathForward(movementSpeed);
                }
            }
            else if (!checkpointSystem.onCheckpoint)
            {
                reverseTimer -= Time.deltaTime;

                if (reverseTimer <= 0)
                {
                    FollowPathReverse(payloadStats.reverseSpeed);
                }
            }
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

    private void FollowPathForward(float speed)
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

    private void FollowPathReverse(float speed)
    {
        if (currentNodeID > 0)
        {
            movementSpeed = -speed;

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
