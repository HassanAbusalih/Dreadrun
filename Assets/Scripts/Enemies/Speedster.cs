using System.Collections;
using UnityEditor;
using UnityEngine;

public class Speedster : EnemyAIBase
{
    [SerializeField] float shootingRange = 7.0f;
    [SerializeField] float retreatRange = 3.0f;
    [SerializeField] float shotCooldown = 1f;
    [SerializeField] float distanceToTravelBeforeShooting = 10f;
    [SerializeField] Vector3 positionAroundTarget = Vector3.zero;
    [SerializeField] float distanceTravelled;
    Node topNode;
    Coroutine stopAndShoot;

    private void Start()
    {
        GeneratePosition();
        MoveToTarget moveToTarget = new(this, GetClosestPlayer, movementSpeed);
        MoveToTarget retreat = new(this, GetClosestPlayer, -movementSpeed);
        SpecializedMovement specializedMovement = new(this, GetClosestPlayer);

        TargetRangeCheck retreatRange = new(this, GetClosestPlayer, 0, this.retreatRange);
        TargetRangeCheck shootingRange = new(this, GetClosestPlayer, this.retreatRange, this.shootingRange);
        TargetRangeCheck farRange;
        if (payload != null)
        {
            farRange = new(this, payload, this.shootingRange, float.MaxValue);
        }
        else
        {
            farRange = new(this, GetClosestPlayer(), this.shootingRange, float.MaxValue);
        }

        Sequencer retreatSeq = new(retreatRange, retreat);
        Sequencer shootingSeq = new(shootingRange, specializedMovement);
        Sequencer farSeq = new(farRange, moveToTarget);

        topNode = new Selector(new Node[] { retreatSeq, shootingSeq, farSeq });
    }

    void Update()
    {
        topNode.Execute();
    }

    public override void SpecializedMovement(Transform player)
    {
        if (player == null)
        {
            return;
        }
        else if (retreating)
        {
            GeneratePosition();
            retreating = false;
        }
        if (distanceTravelled >= distanceToTravelBeforeShooting)
        {
            if (stopAndShoot == null) 
            { 
                stopAndShoot = StartCoroutine(StopAndShoot(player)); 
            }
            else 
            { 
                rb.velocity = new(0, rb.velocity.y, 0); 
            }
            return;
        }
        Vector3 targetPos = player.position + positionAroundTarget;
        if (Vector3.Distance(targetPos, transform.position) < 1f)
        {
            GeneratePosition();
        }
        Vector3 toTarget = (targetPos - transform.position).normalized * movementSpeed;
        transform.rotation = Quaternion.LookRotation(player.position - transform.position, Vector3.up);
        rb.velocity = new(toTarget.x, rb.velocity.y, toTarget.z);
        distanceTravelled += rb.velocity.magnitude * Time.deltaTime;
    }

    void GeneratePosition()
    {
        Debug.Log("Hello!");
        positionAroundTarget = Random.insideUnitSphere * shootingRange;
        positionAroundTarget.y = 0f;
    }

    IEnumerator StopAndShoot(Transform target)
    {
        Vector3 toTarget = (target.position - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(toTarget, Vector3.up);
        yield return new WaitForSeconds(shotCooldown / 2);
        transform.rotation = Quaternion.LookRotation(toTarget, Vector3.up);
        weapon.Attack();
        yield return new WaitForSeconds(shotCooldown / 2);
        distanceTravelled = 0;
        stopAndShoot = null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, shootingRange);
        Gizmos.color = new Color(0, 0, 0, 0.2f);
        Gizmos.DrawSphere(transform.position, shootingRange);
        Handles.color = new Color(1, 0, 0, 0.2f);
        Handles.DrawSolidDisc(transform.position, Vector3.up, shootingRange);
        Handles.color = new Color(1, 0, 0, 0.2f);
        Handles.DrawSolidDisc(transform.position, Vector3.up, retreatRange);
    }
}