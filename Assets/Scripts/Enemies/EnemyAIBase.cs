using UnityEngine;

public abstract class EnemyAIBase : MonoBehaviour, IDamagable
{
    [SerializeField] int maxHealth = 5;
    protected WeaponBase weapon;
    protected Transform payload;
    protected Transform[] players;
    int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
        gameObject.layer = 7;
        weapon = GetComponent<WeaponBase>();
    }

    public void TakeDamage(int Amount)
    {
        currentHealth -= Amount;
        if (currentHealth == 0) { Destroy(gameObject); }
    }

    public void Initialize(Transform payload, Transform[] players)
    {
        this.payload = payload;
        this.players = players;
    }

    protected virtual Transform GetClosestPlayer()
    {
        Transform closestPlayer = null;
        float closestDistance = float.MaxValue;
        foreach (Transform player in players)
        {
            float distance = Vector3.Distance(transform.position, player.position);
            if (distance < closestDistance)
            {
                closestPlayer = player;
                closestDistance = distance;
            }
        }
        return closestPlayer;
    }
}
