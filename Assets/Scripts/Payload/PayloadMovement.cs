using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PayloadMovement : MonoBehaviour
{
    [Header("Phase Change Settings")]
    public bool movementEnabled = false;
    Animator payloadAnimator;

    [Header("Path Follow Settings")]
    [SerializeField] PayloadPath currentPath;
    [SerializeField] float wayPointProximity =0.2f;
    [SerializeField] float rotationSpeed, movementSpeed;
    Rigidbody payloadRigidbody;
    int currentNodeID = 0;

    [Header("Reverse Settings")]
    [SerializeField] float reverseCountDownTime;
    float reverseTimer;

    [Header("Object Detection Settings")]
    [SerializeField] float payloadRange;
    [SerializeField] LayerMask enemyLayer;
    [SerializeField] GameObject rangeIndicator;
    PayloadUI payloadUI;
    Player[] playersInGame;
    int playersOnPayload;
    bool enemyInRange;

    [Header("Checkpoint Settings")]
    PayloadCheckpointSystem checkpointSystem;
    int lastCheckpointNodeID = 0;

    [Header("Payload Stats")]
    PayloadStats payloadStats;



    private void OnValidate()
    {
        rangeIndicator.transform.localScale = new Vector3(payloadRange / transform.localScale.x, .1f / transform.localScale.y, payloadRange / transform.localScale.z);
    }
    private void Start()
    {
        payloadStats = gameObject.GetComponent<PayloadStats>();
        checkpointSystem = gameObject.GetComponent<PayloadCheckpointSystem>();
        reverseTimer = reverseCountDownTime;
        currentPath = FindObjectOfType<PayloadPath>();
        playersInGame = FindObjectsOfType<Player>();
        payloadAnimator = GetComponent<Animator>();
        payloadRigidbody = GetComponent<Rigidbody>();
        payloadUI = FindObjectOfType<PayloadUI>();
        enemyLayer = LayerMask.GetMask("Enemy");
    }

    private void Update()
    {
        if(wayPointProximity < 0.1)
        {
            Debug.Log("Way point size too small");
        }

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
                    payloadUI.ChangePayloadUISprite(playersOnPayload);
                }
                else
                {
                    payloadUI.ChangePayloadUISprite(0);
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
        if (currentNodeID < currentPath.pathNodes.Count)
        {
            float node_Distance = Vector3.Distance(currentPath.pathNodes[currentNodeID].position, transform.position);
            payloadRigidbody.velocity = (currentPath.pathNodes[currentNodeID].position - transform.position).normalized * speed;

            Quaternion target_Rotation = Quaternion.LookRotation(currentPath.pathNodes[currentNodeID].position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, target_Rotation, Time.deltaTime * rotationSpeed);

            if (node_Distance <= wayPointProximity)
            {
                currentNodeID++;
                if (currentPath.pathNodes[currentNodeID - 1].gameObject.CompareTag("Checkpoint") && currentNodeID > lastCheckpointNodeID)
                {
                    lastCheckpointNodeID = currentNodeID;
                    StartCoroutine(checkpointSystem.ActivateCheckpoint());
                }
            }
        }
    }

    private void FollowPathReverse(float speed)
    {
        if (currentNodeID > 0)
        {
            movementSpeed = -speed;

            payloadUI.ChangePayloadUISprite(4);

            float node_Distance = Vector3.Distance(currentPath.pathNodes[currentNodeID - 1].position, transform.position);
            payloadRigidbody.velocity = (currentPath.pathNodes[currentNodeID - 1].position - transform.position).normalized * speed;

            Quaternion target_Rotation = Quaternion.LookRotation(currentPath.pathNodes[currentNodeID].position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, target_Rotation, Time.deltaTime * rotationSpeed);

            if (node_Distance <= wayPointProximity)
            {
                currentNodeID--;
            }
        }
    }

    public void EnableMovement()
    {
        movementEnabled = true;
        if (payloadAnimator == null) return;
        payloadAnimator.SetTrigger("Artifact Enable");
    }
}
