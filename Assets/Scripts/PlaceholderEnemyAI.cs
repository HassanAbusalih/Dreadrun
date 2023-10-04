using UnityEngine;

public class PlaceholderEnemyAI : MonoBehaviour, IDamagable
{
    [SerializeField] int maxHealth = 5;
    int currentHealth;
    [SerializeField] Transform payload;
    [SerializeField] Transform player;
    [SerializeField] float distanceFromPayload = 5.0f;
    [SerializeField] float movementSpeed = 3.0f;
    [SerializeField] float maxRotationAngle = 10f;
    [SerializeField] EnemyWeapon weapon;

    private void Start()
    {
        currentHealth = maxHealth;
        gameObject.layer = 7;
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        Vector3 toEnemy = transform.position - payload.position;
        Vector3 toPlayer = player.position - transform.position;
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
        transform.position = Vector3.MoveTowards(transform.position, desiredPosition, movementSpeed * Time.deltaTime);
    }

    public void TakeDamage(int Amount)
    {
        currentHealth -= Amount;
        if (currentHealth == 0) { Destroy(gameObject); }
    }
}
