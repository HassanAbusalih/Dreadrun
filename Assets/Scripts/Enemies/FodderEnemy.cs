using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(FlockingBehavior))]
public class FodderEnemy : EnemyAIBase
{
    [SerializeField] float shootingRange = 7.0f;
    [SerializeField] float strafeRange = 5.0f;
    [SerializeField] float retreatRange = 3.0f;
    [SerializeField] float delayBetweenShots = 1f;
    float strafeStartTime;
    float strafeLength;
    bool clockwiseStrafe;
    Node topNode;
    [SerializeField][Range(0, 1f)] float strafePercent;
    [SerializeField][Range(0, 1f)] float flockPercent;

    private void Start()
    {
        if (flockingBehavior == null)
        {
            gameObject.AddComponent<FlockingBehavior>();
        }
        MoveToTarget moveToTarget = new(this, GetClosestPlayer, movementSpeed);
        MoveToTarget retreat = new(this, GetClosestPlayer, -movementSpeed);
        AttackInterval attackInterval = new(this, delayBetweenShots);
        SpecializedMovement specializedMovement = new(this, GetClosestPlayer);

        TargetRangeCheck retreatRange = new(this, GetClosestPlayer, 0, this.retreatRange);
        TargetRangeCheck strafeRange = new(this, GetClosestPlayer, 0, this.strafeRange);
        TargetRangeCheck shootingRange = new(this, GetClosestPlayer, this.strafeRange, this.shootingRange);
        TargetRangeCheck farRange;
        if (payload != null)
        {
            farRange = new(this, payload, this.shootingRange, float.MaxValue);
        }
        else
        {
            farRange = new(this, GetClosestPlayer(), this.shootingRange, float.MaxValue);
        }

        Sequencer retreatSeq = new(retreatRange, retreat, attackInterval);
        Sequencer strafeSeq = new(strafeRange, specializedMovement, attackInterval);
        Sequencer withinShootingRangeSeq = new(shootingRange, moveToTarget, attackInterval);
        Sequencer farSeq = new(farRange, moveToTarget);

        topNode = new Selector(new Node[] { retreatSeq, strafeSeq, withinShootingRangeSeq, farSeq});
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
        else if (retreating)
        {
            Move(target, -movementSpeed);
        }
        Vector3 toTarget = (target.position - transform.position).normalized;
        Vector3 strafeDirection = Vector3.Cross(toTarget, Vector3.up);
        Vector3 flockingForce = flockingBehavior.Flocking(movementSpeed);
        if (!clockwiseStrafe)
        {
            strafeDirection = -strafeDirection;
        }
        Vector3 strafeVector = ((strafePercent * strafeDirection) + (flockPercent * flockingForce)).normalized * movementSpeed;
        transform.rotation = Quaternion.LookRotation(toTarget, Vector3.up);
        rb.velocity = Vector3.Lerp(rb.velocity, new Vector3(strafeVector.x, rb.velocity.y, strafeVector.z), (Time.time - strafeStartTime) / strafeLength);
    }

    void ResetStrafe()
    {
        strafeLength = Random.Range(2f, 10f);
        strafeStartTime = Time.time;
        clockwiseStrafe = (Random.value > 0.5f);
    }
}
