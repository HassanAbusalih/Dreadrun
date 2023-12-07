using UnityEngine;

public class Homing : MonoBehaviour
{
    float homingStrength;
    float homingRange;
    bool enemyHoming = false;
    EnemyAIBase enemyTarget;
    Player playerTarget;

    public void Initialize(float homingStrength, float homingRange, bool enemyHoming = false)
    {
        this.homingStrength = homingStrength;
        this.homingRange = homingRange;
        this.enemyHoming = enemyHoming;
    }

    void FixedUpdate()
    {
        if (!enemyHoming)
        {
            HomeOnEnemy();
        }
        else
        {
            if (playerTarget == null)
            {
                playerTarget = FindObjectOfType<Player>();
            }
            if (playerTarget != null)
            {
                Vector3 directionToEnemy = playerTarget.transform.position - transform.position;
                Quaternion targetRotation = Quaternion.LookRotation(directionToEnemy);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, homingStrength * Time.deltaTime);
            }
        }
    }

    private void HomeOnEnemy()
    {
        if (enemyTarget == null)
        {
            enemyTarget = FindClosestEnemy();
        }
        if (enemyTarget != null)
        {
            Vector3 directionToEnemy = enemyTarget.transform.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(directionToEnemy);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, homingStrength * Time.deltaTime);
        }
    }

    EnemyAIBase FindClosestEnemy()
    {
        EnemyAIBase closest = null;
        float closestDistance = Mathf.Infinity;
        if (EnemyPool.Instance == null) { return null; }
        foreach (EnemyAIBase enemy in EnemyPool.Instance.Enemies)
        {
            if (enemy == null) { continue; }
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance && distance <= homingRange)
            {
                closest = enemy;
                closestDistance = distance;
            }
        }
        return closest;
    }
}