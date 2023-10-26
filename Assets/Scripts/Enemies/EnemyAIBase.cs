using UnityEngine;

public abstract class EnemyAIBase : MonoBehaviour, IDamagable
{
    [SerializeField] float maxHealth = 5;
    protected WeaponBase weapon;
    protected Transform payload;
    protected Transform[] players = new Transform[0];
    protected Rigidbody rb;
    float currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
        gameObject.layer = 7;
        weapon = GetComponent<WeaponBase>();
        rb = GetComponent<Rigidbody>();
        if (players.Length == 0)
        {
            GetPlayers();
        }
    }

    protected void GetPlayers()
    {
        Player[] players = FindObjectsOfType<Player>();
        this.players = new Transform[players.Length];
        for (int i = 0; i < this.players.Length; i++)
        {
            this.players[i] = players[i].transform;
        }
    }

    public void TakeDamage(float Amount)
    {
        currentHealth -= Amount;
        if (currentHealth <= 0) { Destroy(gameObject); }
    }

    public void Initialize(Transform payload, Transform[] players)
    {
        this.payload = payload;
        this.players = players;
    }

    protected virtual Transform GetClosestPlayer()
    {
        if (players.Length == 0) {  return null; }
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
