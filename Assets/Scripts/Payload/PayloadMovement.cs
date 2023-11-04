using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PayloadMovement : MonoBehaviour
{
    [Header("Phase Change Settings")]
    public bool movementEnabled = false;
    private Animator payloadAnimator;

    [Header("Path Follow Settings")]
    [SerializeField] private PayloadPath currentPath;
    [SerializeField] private float waypointProximity = 0.2f;
    [SerializeField] private float rotationSpeed, movementSpeed;
    private Rigidbody payloadRigidbody;
    private int NextWayPoint;
    private bool isMovingForward;

    [Header("Reverse Settings")]
    [SerializeField] private float reverseCountDownTime;
    private float reverseTimer;

    [Header("Object Detection Settings")]
    [SerializeField] private float payloadRange;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private GameObject rangeIndicator;
    private PayloadUI payloadUI;
    private Player[] playersInGame;
    private int playersOnPayload;
    private bool enemyInRange;

    [Header("Checkpoint Settings")]
    private PayloadCheckpointSystem checkpointSystem;
    private int lastCheckpointNodeID = 0;

    [Header("Payload Stats")]
    private PayloadStats payloadStats;

    private void OnValidate()
    {
        UpdateRangeIndicatorScale();
    }

    private void Start()
    {
        InitializeComponents();
        reverseTimer = reverseCountDownTime;
        currentPath = FindObjectOfType<PayloadPath>();
        playersInGame = FindObjectsOfType<Player>();
    }

    private void Update()
    {
        if (movementEnabled)
        {
            UpdateRangeIndicatorScale();
            RotateTowardsPath();
            StartCoroutine(ObjectsInRangeCheck());

            if (playersOnPayload > 0 && NextWayPoint < currentPath.pathNodes.Count && !checkpointSystem.onCheckpoint)
            {
                reverseTimer = reverseCountDownTime;

                if (!enemyInRange)
                {
                    FollowPathForward(movementSpeed);
                }
                else
                {
                    StopPayload();
                }
            }
            else if (!checkpointSystem.onCheckpoint)
            {
                reverseTimer -= Time.deltaTime;

                StopPayload();

                if (reverseTimer <= 0)
                {
                    FollowPathReverse(payloadStats.reverseSpeed);
                }
            }
        }
    }

    private void UpdateRangeIndicatorScale()
    {
        // Update the scale of the range indicator to match the payload's range
        rangeIndicator.transform.localScale = new Vector3(payloadRange / transform.localScale.x, .1f / transform.localScale.y, payloadRange / transform.localScale.z);
    }

    private void RotateTowardsPath()
    {
        Quaternion targetRotation = Quaternion.identity;

        if (isMovingForward)
        {
            targetRotation = Quaternion.LookRotation(currentPath.pathNodes[NextWayPoint].position - transform.position);
        }
        else if (NextWayPoint > 0)
        {
            targetRotation = Quaternion.LookRotation(transform.position - currentPath.pathNodes[NextWayPoint - 1].position);
        }

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

    private IEnumerator ObjectsInRangeCheck()
    {
        // Check for nearby enemies and players on the payload
        Collider[] enemiesInRange = Physics.OverlapSphere(transform.position, payloadRange, enemyLayer);

        if (enemiesInRange.Length > 0)
        {
            enemyInRange = true;
        }
        else
        {
            enemyInRange = false;
        }

        playersOnPayload = CountPlayersOnPayload();

        // Determine movement speed based on the number of players on the payload
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

        yield return new WaitForSeconds(0.01f);
    }

    private int CountPlayersOnPayload()
    {
        // Count the number of players within the payload's range
        int count = 0;
        foreach (Player player in playersInGame)
        {
            if (Vector3.Distance(transform.position, player.transform.position) < payloadRange)
            {
                count++;
            }
        }
        return count;
    }

    private void FollowPathForward(float speed)
    {
        // Move the payload forward along the path
        if (NextWayPoint < currentPath.pathNodes.Count)
        {
            isMovingForward = true;

            payloadUI.ChangePayloadStateDisplay(playersOnPayload);

            float nodeDistance = Vector3.Distance(currentPath.pathNodes[NextWayPoint].position, transform.position);
            payloadRigidbody.velocity = (currentPath.pathNodes[NextWayPoint].position - transform.position).normalized * speed;

            if (nodeDistance <= waypointProximity)
            {
                NextWayPoint++;
                if (currentPath.pathNodes[NextWayPoint - 1].gameObject.CompareTag("Checkpoint") && NextWayPoint > lastCheckpointNodeID)
                {
                    lastCheckpointNodeID = NextWayPoint;
                    StartCoroutine(checkpointSystem.ActivateCheckpoint());
                }
            }
        }
    }

    private void FollowPathReverse(float speed)
    {
        // Move the payload in reverse along the path
        if (NextWayPoint > 0)
        {
            isMovingForward = false;

            movementSpeed = -speed;

            payloadUI.ChangePayloadStateDisplay(4);

            float nodeDistance = Vector3.Distance(currentPath.pathNodes[NextWayPoint - 1].position, transform.position);
            payloadRigidbody.velocity = (currentPath.pathNodes[NextWayPoint - 1].position - transform.position).normalized * speed;

            if (nodeDistance <= waypointProximity)
            {
                NextWayPoint--;
            }
        }
    }

    private void StopPayload()
    {
        // Stop the payload's movement
        payloadUI.ChangePayloadStateDisplay(0);

        if (isMovingForward)
        {
            float nodeDistance = Vector3.Distance(currentPath.pathNodes[NextWayPoint].position, transform.position);
            payloadRigidbody.velocity = (currentPath.pathNodes[NextWayPoint].position - transform.position).normalized * payloadRigidbody.velocity.magnitude;

            if (nodeDistance <= waypointProximity)
            {
                NextWayPoint++;
                if (currentPath.pathNodes[NextWayPoint - 1].gameObject.CompareTag("Checkpoint") && NextWayPoint > lastCheckpointNodeID)
                {
                    lastCheckpointNodeID = NextWayPoint;
                    StartCoroutine(checkpointSystem.ActivateCheckpoint());
                }
            }
        }
        else
        {
            float nodeDistance = Vector3.Distance(currentPath.pathNodes[NextWayPoint - 1].position, transform.position);
            payloadRigidbody.velocity = (currentPath.pathNodes[NextWayPoint - 1].position - transform.position).normalized * payloadRigidbody.velocity.magnitude;

            if (nodeDistance <= waypointProximity)
            {
                NextWayPoint--;
            }
        }
    }

    public void EnableMovement()
    {
        // Enable the payload's movement and trigger the artifact animation
        movementEnabled = true;
        if (payloadAnimator == null) return;
        payloadAnimator.SetTrigger("Artifact Enable");
    }

    private void InitializeComponents()
    {
        // Initialize required components
        payloadStats = gameObject.GetComponent<PayloadStats>();
        checkpointSystem = gameObject.GetComponent<PayloadCheckpointSystem>();
        payloadAnimator = GetComponent<Animator>();
        payloadRigidbody = GetComponent<Rigidbody>();
        payloadUI = FindObjectOfType<PayloadUI>();
        enemyLayer = LayerMask.GetMask("Enemy");
    }
}
