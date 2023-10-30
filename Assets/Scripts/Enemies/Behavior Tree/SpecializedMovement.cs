using UnityEngine;

public class SpecializedMovement : Node
{
    EnemyAIBase enemyAI;
    public delegate Transform SpecializedMovementDelegate();
    SpecializedMovementDelegate target;

    public SpecializedMovement(EnemyAIBase enemyAI, SpecializedMovementDelegate target)
    {
        this.enemyAI = enemyAI;
        this.target = target;
    }

    public override NodeState Execute()
    {
        if (target == null)
        {
            return NodeState.Failure;
        }
        enemyAI.SpecializedMovement(target());
        return NodeState.Success;
    }
}