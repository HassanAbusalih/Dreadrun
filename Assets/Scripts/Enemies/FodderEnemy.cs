using UnityEngine;

public class FodderEnemy : EnemyAIBase
{
    [SerializeField] float shootingRange = 7f;
    [SerializeField] float strafeRange = 5f;
    [SerializeField] float movementSpeed = 3f;
    [SerializeField] float delayBetweenShots = 1f;
    float strafeStartTime;
    float strafeLength;
    bool clockwiseStrafe;
    FlockingBehavior flockingBehavior;
    Node topNode;

    private void Start()
    {
        if (!TryGetComponent(out flockingBehavior))
        {
            gameObject.AddComponent<FlockingBehavior>();
        }
        MoveToTarget moveToTarget = new(this, GetClosestPlayer, movementSpeed);
        AttackInterval attackInterval = new(this, delayBetweenShots);
        SpecializedMovement specializedMovement = new(this, GetClosestPlayer);
        FlockingNode flockingNode = new(flockingBehavior);

        TargetInRange targetInRange = new(this, GetClosestPlayer, shootingRange);
        TargetIsFar targetIsFar;
        if (payload != null)
        {
            targetIsFar = new(this, payload, shootingRange);
        }
        else
        {
            targetIsFar = new(this, GetClosestPlayer(), shootingRange);
        }

        Sequencer attackAndMoveSeq = new(targetInRange, attackInterval, moveToTarget);
        Sequencer moveToTargetSeq = new(targetIsFar, moveToTarget);
        Sequencer specializedMovementSeq = new(targetInRange, specializedMovement, attackInterval);

        topNode = new Selector(moveToTargetSeq, attackAndMoveSeq, specializedMovementSeq, flockingNode);
    }

    void Update()
    {
        topNode.Execute();
        if (Time.time - strafeStartTime >= strafeLength)
        {
            ResetStrafe();
        }
    }

    public override void SpecializedMovement(Transform target)
    {
        if (target == null)
        {
            return;
        }
        Vector3 toTarget = (target.position - transform.position).normalized;
        Vector3 strafeDirection = Vector3.Cross(toTarget, Vector3.up);

        if (!clockwiseStrafe)
        {
            strafeDirection = -strafeDirection;
        }
        Vector3 strafeVector = movementSpeed * Time.deltaTime * strafeDirection;
        rb.velocity = new Vector3(strafeVector.x, rb.velocity.y, strafeVector.z);
    }

    void ResetStrafe()
    {
        strafeLength = Random.Range(2f, 10f);
        strafeStartTime = Time.time;
        clockwiseStrafe = (Random.value > 0.5f);
    }
}
