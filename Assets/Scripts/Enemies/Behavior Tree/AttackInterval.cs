using UnityEngine;

public class AttackInterval : Node
{
    EnemyAIBase enemyAI;
    float lastAttackTime;
    float attackInterval;
    float currentInterval;
    float percentageVariant;

    public AttackInterval(EnemyAIBase enemyAI, float attackInterval, float percentageVariant)
    {
        this.enemyAI = enemyAI;
        this.attackInterval = attackInterval;
        this.percentageVariant = percentageVariant;
        currentInterval = attackInterval + (Random.Range(-percentageVariant, percentageVariant) * attackInterval);
    }

    public override NodeState Execute()
    {
        if (Time.time - lastAttackTime > currentInterval)
        {
            enemyAI.Attack();
            lastAttackTime = Time.time;
            currentInterval = attackInterval + (Random.Range(-percentageVariant, percentageVariant) * attackInterval);
            return NodeState.Success;
        }

        return NodeState.Running;
    }
}