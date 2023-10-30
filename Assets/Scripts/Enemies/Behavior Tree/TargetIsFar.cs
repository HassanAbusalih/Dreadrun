using UnityEngine;

public class TargetIsFar : Node
{
    EnemyAIBase enemyAI;
    Transform target;
    float range;

    public TargetIsFar(EnemyAIBase enemyAI, Transform target, float range)
    {
        this.enemyAI = enemyAI;
        this.target = target;
        this.range = range;
    }

    public override NodeState Execute()
    {
        if (target == null)
        {
            return NodeState.Failure;
        }
        float distance = Vector3.Distance(enemyAI.transform.position, target.position);
        if (distance > range)
        {
            return NodeState.Success;
        }
        return NodeState.Failure;
    }
}