using System.Collections;
using System.Linq;
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
    private int nextWayPointIndex;
    private bool isMovingForward;

    [Header("Reverse Settings")]
    [SerializeField] private float reverseCountDownTime;
    private float reverseTimer;

    [Header("Object Detection Settings")]
    [SerializeField] private LayerMask enemyLayer;
    private PayloadUI payloadUI;
    private Player[] playersInGame;
    private int playersOnPayload;
    private bool enemyInRange;

    [Header("Checkpoint Settings")]
    private PayloadCheckpointSystem checkpointSystem;
    private int lastCheckpointNodeID = 0;

    private void Start()
    {
        InitializeVariables();
    }

    private void Update()
    {
        if (movementEnabled)
        {
            RotateTowardsPath();

            StartCoroutine(CheckForObjectsInRange());

            if (playersOnPayload > 0)
            {
                reverseTimer = reverseCountDownTime;

                if (!enemyInRange)
                    FollowPathForward(movementSpeed);
                else
                    StopPayload();
            }
            else if (nextWayPointIndex > 0)
            {
                reverseTimer -= Time.deltaTime;

                if (reverseTimer <= 0)
                    FollowPathReverse(PayloadStats.instance.reverseSpeed);
                else
                    StopPayload();
            }
        }
    }

    private void FollowPathForward(float speed)
    {
        // Move the payload forward along the path
        isMovingForward = true;

        payloadUI.ChangePayloadStateDisplay(playersOnPayload);

        if (CheckWaypointProximity(currentPath.pathNodes[nextWayPointIndex].position, waypointProximity))
        {
            nextWayPointIndex++;
            payloadUI.UpdateLastWayPointIndex(nextWayPointIndex - 1);

            CheckForCheckpoint();
        }

        if (nextWayPointIndex >= currentPath.pathNodes.Count)
        {
            payloadRigidbody.velocity = Vector3.zero;
            movementEnabled = false;
            GameManager.Instance.Win();
        }
        else
        {
            MovePayload(currentPath.pathNodes[nextWayPointIndex].position, speed);
        }
    }

    private void FollowPathReverse(float speed)
    {
        // Move the payload in reverse along the path
        isMovingForward = false;

        payloadUI.ChangePayloadStateDisplay(4);

        if (CheckWaypointProximity(currentPath.pathNodes[nextWayPointIndex - 1].position, waypointProximity))
        {
            nextWayPointIndex--;
            payloadUI.UpdateLastWayPointIndex(nextWayPointIndex);
        }

        if (nextWayPointIndex <= 0)
        {
            payloadRigidbody.velocity = Vector3.zero;
            movementEnabled = false;
            GameManager.Instance.Lose();
        }
        else
        {
            MovePayload(currentPath.pathNodes[nextWayPointIndex - 1].position, speed);
        }
    }

    private void StopPayload()
    {
        // Stop the payload's movement using the payload's rigidbody
        payloadUI.ChangePayloadStateDisplay(0);

        if (payloadRigidbody.velocity.magnitude > 0)
        {
            RotateTowardsPath();

            if (isMovingForward && nextWayPointIndex < currentPath.pathNodes.Count)
            {
                if (CheckWaypointProximity(currentPath.pathNodes[nextWayPointIndex].position, waypointProximity))
                {
                    nextWayPointIndex++;
                    payloadUI.UpdateLastWayPointIndex(nextWayPointIndex - 1);

                    CheckForCheckpoint();
                }

                MovePayload(currentPath.pathNodes[nextWayPointIndex].position, payloadRigidbody.velocity.magnitude);
            }
            else if (!isMovingForward && nextWayPointIndex > 0)
            {
                if (CheckWaypointProximity(currentPath.pathNodes[nextWayPointIndex - 1].position, waypointProximity))
                {
                    nextWayPointIndex--;
                    payloadUI.UpdateLastWayPointIndex(nextWayPointIndex);
                }

                MovePayload(currentPath.pathNodes[nextWayPointIndex - 1].position, payloadRigidbody.velocity.magnitude);
            }
        }
    }

    private void MovePayload(Vector3 destination, float speed)
    {
        Vector3 direction = (destination - transform.position).normalized;
        payloadRigidbody.velocity = direction * speed;
    }

    private bool CheckWaypointProximity(Vector3 waypoint, float proximity)
    {
        float nodeDistance = Vector3.Distance(waypoint, transform.position);
        return nodeDistance <= proximity;
    }

    private void CheckForCheckpoint()
    {
        if (currentPath.pathNodes[nextWayPointIndex - 1].gameObject.CompareTag("Checkpoint") && nextWayPointIndex > lastCheckpointNodeID)
        {
            lastCheckpointNodeID = nextWayPointIndex;
            StartCoroutine(checkpointSystem.ActivateCheckpoint());
        }
    }

    private IEnumerator CheckForObjectsInRange()
    {
        // Check for enemies within the payload's range
        Collider[] enemiesInRange = Physics.OverlapSphere(transform.position, PayloadStats.instance.payloadRange, enemyLayer);

        enemyInRange = enemiesInRange.Length > 0;

        // Count the number of players within the payload's range
        playersOnPayload = playersInGame.Count(player => Vector3.Distance(transform.position, player.transform.position) < PayloadStats.instance.payloadRange);

        // Determine movement speed based on the number of players within the payload's range
        movementSpeed = playersOnPayload switch
        {
            0 => 0,
            1 => PayloadStats.instance.onePlayerSpeed,
            2 => PayloadStats.instance.twoPlayerSpeed,
            _ => PayloadStats.instance.threePlayerSpeed
        };

        yield return new WaitForSeconds(0.01f);
    }

    private void RotateTowardsPath()
    {
        Quaternion targetRotation = Quaternion.identity;

        if (isMovingForward && nextWayPointIndex < currentPath.pathNodes.Count)
        {
            targetRotation = Quaternion.LookRotation(currentPath.pathNodes[nextWayPointIndex].position - transform.position);
        }
        else if (nextWayPointIndex > 0)
        {
            targetRotation = Quaternion.LookRotation(transform.position - currentPath.pathNodes[nextWayPointIndex - 1].position);
        }

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

    void EnableMovement()
    {
        // Enable the payload's movement and trigger the artifact animation
        movementEnabled = true;
        if (payloadAnimator == null) return;
        payloadUI = FindObjectOfType<PayloadUI>();
        payloadAnimator.SetTrigger("Artifact Enable");
    }

    void DisableMovement()
    {
        // Stop the payload and disable the payload's movement
        payloadRigidbody.velocity = Vector3.zero;
        RotateTowardsPath();
        payloadUI.ChangePayloadStateDisplay(0);
        movementEnabled = false;
    }

    private void InitializeVariables()
    {
        // Initialize required variables
        checkpointSystem = gameObject.GetComponent<PayloadCheckpointSystem>();
        payloadAnimator = GetComponent<Animator>();
        payloadRigidbody = GetComponent<Rigidbody>();
        enemyLayer = LayerMask.GetMask("Enemy");
        reverseTimer = reverseCountDownTime;
        currentPath = FindObjectOfType<PayloadPath>();
        playersInGame = FindObjectsOfType<Player>() ?? null;
        payloadUI = FindObjectOfType<PayloadUI>() ?? null;
        GameManager.Instance.onPhaseChange.AddListener(EnableMovement);
        PayloadCheckpointSystem.Instance.onCheckpointActivate.AddListener(DisableMovement);
        PayloadCheckpointSystem.Instance.onCheckpointDeactivate.AddListener(EnableMovement);
    }
}
