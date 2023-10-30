using UnityEngine;

public class MoveToTarget : Node
{
    EnemyAIBase enemyAI;
    public delegate Transform MoveToTargetDelegate();
    MoveToTargetDelegate target;
    float speed;

    public MoveToTarget(EnemyAIBase enemyAI, MoveToTargetDelegate target, float speed)
    {
        this.enemyAI = enemyAI;
        this.target = target;
        this.speed = speed;
    }

    public override NodeState Execute()
    {
        Transform target = this.target();
        if (target == null)
        {
            Debug.Log("MoveToTarget: Target is null");
            return NodeState.Failure;
        }
        enemyAI.Move(target, speed);
        Debug.Log("MoveToTarget: Moving towards target. Target is: " + target);
        return NodeState.Success;
    }
}