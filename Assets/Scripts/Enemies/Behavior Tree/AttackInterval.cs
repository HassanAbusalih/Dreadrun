using UnityEngine;

public class AttackInterval : Node
{
    EnemyAIBase enemyAI;
    float lastAttackTime;
    float attackInterval;

    public AttackInterval(EnemyAIBase enemyAI, float attackInterval)
    {
        this.enemyAI = enemyAI;
        this.attackInterval = attackInterval;
    }

    public override NodeState Execute()
    {
        if (Time.time - lastAttackTime > attackInterval)
        {
            enemyAI.Attack();
            lastAttackTime = Time.time;
            return NodeState.Success;
        }

        return NodeState.Running;
    }
}