using System.Collections;
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
    private int NextWayPointIndex;
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
            else
            {
                reverseTimer -= Time.deltaTime;

                if (reverseTimer <= 0)
                    FollowPathReverse(PayloadStats.instance.reverseSpeed);
                else
                    StopPayload();
            }
        }
    }

    private void RotateTowardsPath()
    {
        Quaternion targetRotation = Quaternion.identity;

        if (isMovingForward && NextWayPointIndex < currentPath.pathNodes.Count)
        {
            targetRotation = Quaternion.LookRotation(currentPath.pathNodes[NextWayPointIndex].position - transform.position);
        }
        else if (NextWayPointIndex > 0)
        {
            targetRotation = Quaternion.LookRotation(transform.position - currentPath.pathNodes[NextWayPointIndex - 1].position);
        }

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

    private IEnumerator CheckForObjectsInRange()
    {
        // Check for nearby enemies and players on the payload
        Collider[] enemiesInRange = Physics.OverlapSphere(transform.position, PayloadStats.instance.payloadRange, enemyLayer);

        if (enemiesInRange.Length > 0)
        {
            enemyInRange = true;
        }
        else
        {
            enemyInRange = false;
        }

        // Count the number of players within the payload's range
        int playerCount = 0;
        foreach (Player player in playersInGame)
        {
            if (Vector3.Distance(transform.position, player.transform.position) < PayloadStats.instance.payloadRange)
            {
                playerCount++;
            }
        }

        playersOnPayload = playerCount;

        // Determine movement speed based on the number of players on the payload
        switch (playersOnPayload)
        {
            case 1:
                movementSpeed = PayloadStats.instance.onePlayerSpeed;
                break;

            case 2:
                movementSpeed = PayloadStats.instance.twoPlayerSpeed;
                break;

            case 3:
                movementSpeed = PayloadStats.instance.threePlayerSpeed;
                break;
        }

        yield return new WaitForSeconds(0.01f);
    }

    private void FollowPathForward(float speed)
    {
        // Move the payload forward along the path
        isMovingForward = true;

        payloadUI.ChangePayloadStateDisplay(playersOnPayload);

        Vector3 direction = (currentPath.pathNodes[NextWayPointIndex].position - transform.position).normalized;
        payloadRigidbody.velocity = direction * speed;

        float nodeDistance = Vector3.Distance(currentPath.pathNodes[NextWayPointIndex].position, transform.position);

        if (nodeDistance <= waypointProximity)
        {
            NextWayPointIndex++;
            payloadUI.UpdateLastWayPointIndex(NextWayPointIndex - 1);

            if (currentPath.pathNodes[NextWayPointIndex - 1].tag == "Checkpoint" && NextWayPointIndex > lastCheckpointNodeID)
            {
                lastCheckpointNodeID = NextWayPointIndex;
                StartCoroutine(checkpointSystem.ActivateCheckpoint());
            }
        }

        if (NextWayPointIndex >= currentPath.pathNodes.Count)
        {
            payloadRigidbody.velocity = Vector3.zero;
            movementEnabled = false;
            GameManager.Instance.Win();
        }
    }

    private void FollowPathReverse(float speed)
    {
        // Move the payload in reverse along the path
        if (NextWayPointIndex > 0)
        {
            isMovingForward = false;

            movementSpeed = -speed;

            payloadUI.ChangePayloadStateDisplay(4);

            Vector3 direction = (currentPath.pathNodes[NextWayPointIndex - 1].position - transform.position).normalized;
            payloadRigidbody.velocity = direction * speed;

            float nodeDistance = Vector3.Distance(currentPath.pathNodes[NextWayPointIndex - 1].position, transform.position);

            if (nodeDistance <= waypointProximity)
            {
                NextWayPointIndex--;
                payloadUI.UpdateLastWayPointIndex(NextWayPointIndex - 1);
            }
        }
        else
        {
            payloadRigidbody.velocity = Vector3.zero;
            payloadUI.ChangePayloadStateDisplay(0);
        }
    }

    private void StopPayload()
    {
        // Stop the payload's movement
        payloadUI.ChangePayloadStateDisplay(0);

        if (payloadRigidbody.velocity.magnitude > 0)
        {
            RotateTowardsPath();

            if (isMovingForward && NextWayPointIndex < currentPath.pathNodes.Count)
            {
                float nodeDistance = Vector3.Distance(currentPath.pathNodes[NextWayPointIndex].position, transform.position);
                payloadRigidbody.velocity = (currentPath.pathNodes[NextWayPointIndex].position - transform.position).normalized * payloadRigidbody.velocity.magnitude;

                if (nodeDistance <= waypointProximity)
                {
                    NextWayPointIndex++;
                    payloadUI.UpdateLastWayPointIndex(NextWayPointIndex - 1);

                    if (currentPath.pathNodes[NextWayPointIndex - 1].gameObject.CompareTag("Checkpoint") && NextWayPointIndex > lastCheckpointNodeID)
                    {
                        lastCheckpointNodeID = NextWayPointIndex;
                        StartCoroutine(checkpointSystem.ActivateCheckpoint());
                    }
                }
            }
            else if (NextWayPointIndex > 0)
            {
                float nodeDistance = Vector3.Distance(currentPath.pathNodes[NextWayPointIndex - 1].position, transform.position);
                payloadRigidbody.velocity = (currentPath.pathNodes[NextWayPointIndex - 1].position - transform.position).normalized * payloadRigidbody.velocity.magnitude;

                if (nodeDistance <= waypointProximity)
                {
                    NextWayPointIndex--;
                    payloadUI.UpdateLastWayPointIndex(NextWayPointIndex - 1);
                }
            }
        }
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
        playersInGame = FindObjectsOfType<Player>();
        GameManager.Instance.onPhaseChange.AddListener(EnableMovement);
        PayloadCheckpointSystem.Instance.onCheckpointActivate.AddListener(DisableMovement);
        PayloadCheckpointSystem.Instance.onCheckpointDeactivate.AddListener(EnableMovement);
    }
}
