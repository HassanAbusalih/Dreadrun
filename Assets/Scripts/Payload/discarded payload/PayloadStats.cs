using UnityEngine;

public class PayloadStats : MonoBehaviour, IDamagable
{
    public static PayloadStats instance;

    [Header("Speed Settings")]
    public float onePlayerSpeed;
    public float twoPlayerSpeed;
    public float threePlayerSpeed;
    public float reverseSpeed;

    [Header("Health Settings")]
    public float maxPayloadHealth;
    public float payloadHealth;

    [Header("Range Settings")]
    public float payloadRange;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        payloadHealth = maxPayloadHealth;
    }

    public void TakeDamage(float damage)
    {
        payloadHealth -= damage;

        if (payloadHealth <= 0)
        {
            GameManager.Instance.Lose();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, payloadRange);
    }
}
