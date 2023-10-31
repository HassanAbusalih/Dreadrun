using UnityEngine;

public class TargetRangeCheck : Node
{
    EnemyAIBase enemyAI;
    public delegate Transform TargetRangeDelegate();
    TargetRangeDelegate targetDelegate;
    Transform target;
    float minRange;
    float maxRange;

    public TargetRangeCheck(EnemyAIBase enemyAI, TargetRangeDelegate target, float minRange, float maxRange)
    {
        this.enemyAI = enemyAI;
        targetDelegate = target;
        this.minRange = minRange;
        this.maxRange = maxRange;
    }

    public TargetRangeCheck(EnemyAIBase enemyAI, Transform target, float minRange, float maxRange)
    {
        this.enemyAI = enemyAI;
        this.target = target;
        this.minRange = minRange;
        this.maxRange = maxRange;
    }

    public override NodeState Execute()
    {
        Transform currentTarget = null;
        if (targetDelegate != null)
        {
            currentTarget = targetDelegate();
        }
        else if (target != null)
        {
            currentTarget = target;
        }
        if (currentTarget == null)
        {
            Debug.Log("Null target");
            return NodeState.Failure;
        }
        float distance = Vector3.Distance(enemyAI.transform.position, currentTarget.position);
        if (distance >= minRange && distance <= maxRange)
        {
            return NodeState.Success;
        }
        Debug.Log(distance);
        return NodeState.Failure;
    }
}