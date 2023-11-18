using UnityEngine;

public abstract class EnemyAIBase : MonoBehaviour, IDamagable, ISlowable
{
    [SerializeField] float maxHealth = 5;
    [SerializeField] protected float movementSpeed = 3f;
    [SerializeField] protected SoundSO enemySounds;
    [SerializeField] float sfxCooldown = 1f;
    float timeSinceSFX;
    protected WeaponBase weapon;
    protected Transform payload;
    protected Transform[] players = new Transform[0];
    protected Rigidbody rb;
    protected Transform target;
    protected bool retreating;
    protected FlockingBehavior flockingBehavior;
    float currentHealth;

    public bool slowed { get; set; }

    void Awake()
    {
        timeSinceSFX = Time.time;
        currentHealth = maxHealth;
        gameObject.layer = 7;
        weapon = GetComponent<WeaponBase>();
        rb = GetComponent<Rigidbody>();
        flockingBehavior = GetComponent<FlockingBehavior>();
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
        IDamagable.onDamageTaken?.Invoke(gameObject);
        if (Time.time - timeSinceSFX > sfxCooldown && enemySounds != null)
        {
            enemySounds.PlaySound(0, AudioSourceType.Enemy);
            timeSinceSFX = Time.time;
        }
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

    public virtual void Attack()
    {
        weapon?.Attack();
    }

    public virtual void Move(Transform target, float movementSpeed)
    {
        if (movementSpeed < 0)
        {
            retreating = true;
        }
        else
        {
            retreating = false;
        }
        Vector3 moveDirection = (target.position - transform.position).normalized * movementSpeed;
        moveDirection.y = 0;
        transform.rotation = Quaternion.LookRotation(moveDirection);
        rb.velocity = new(moveDirection.x, rb.velocity.y, moveDirection.z);
    }

    public virtual void SpecializedMovement(Transform target)
    {
        transform.LookAt(target.position);
        Vector3 moveDirection = (target.position - transform.position).normalized;
        rb.velocity = new Vector3(moveDirection.x, rb.velocity.y, moveDirection.z);
        Debug.Log("Default Specialized Movement?");
    }

    public void ApplySlow(float slowModifier)
    {
        if (!slowed)
        {
            movementSpeed *= slowModifier;
            slowed = true;
        }
    }

    public void RemoveSlow(float slowModifier)
    {
        if (slowed)
        {
            movementSpeed /= slowModifier;
            slowed = false;
        }
    }
}