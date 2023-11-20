using UnityEngine;

public class Homing : MonoBehaviour
{
    float rotationSpeed;
    float homingRange;
    EnemyAIBase[] enemies;
    EnemyAIBase target;

    void Start()
    {
        enemies = FindObjectsOfType<EnemyAIBase>();
    }

    void FixedUpdate()
    {
        if (target == null)
        {
            target = FindClosestEnemy();
        }
        if (target != null)
        {
            Vector3 directionToEnemy = target.transform.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(directionToEnemy);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    EnemyAIBase FindClosestEnemy()
    {
        EnemyAIBase closest = null;
        float closestDistance = Mathf.Infinity;
        foreach (EnemyAIBase enemy in enemies)
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

    public void Initialize(float rotationSpeed, float homingRange)
    {
        this.rotationSpeed = rotationSpeed;
        this.homingRange = homingRange;
    }
}