using UnityEngine;

public class FodderEnemy : EnemyAIBase
{
    [SerializeField] float distanceFromPayload = 5.0f;
    [SerializeField] float movementSpeed = 3f;
    [SerializeField] float maxRotationAngle = 10f;
    
    void Update()
    {
        Move();
        ShootIfWithinRange();
    }

    private void ShootIfWithinRange()
    {
        if (Vector3.Distance(transform.position, payload.transform.position) <= distanceFromPayload)
        {
            weapon.Attack();
        }
    }

    void Move()
    {
        Vector3 toEnemy = transform.position - payload.position;
        Vector3 toPlayer = GetClosestPlayer().position - transform.position;
        float playerProximity = 1f - Mathf.Clamp01(toPlayer.magnitude / distanceFromPayload);
        float rotationAmount = maxRotationAngle * playerProximity;
        Vector3 rotatedDirection;
        if (Vector3.Cross(toEnemy, toPlayer).y > 0)
        {
            rotatedDirection = Quaternion.Euler(0, -rotationAmount, 0) * toEnemy;
        }
        else
        {
            rotatedDirection = Quaternion.Euler(0, rotationAmount, 0) * toEnemy;
        }
        Vector3 desiredPosition = payload.position + rotatedDirection.normalized * distanceFromPayload;
        desiredPosition = desiredPosition.normalized * movementSpeed * Time.deltaTime;
        rb.velocity = new Vector3(desiredPosition.x, rb.velocity.y, desiredPosition.z);
    }
}
