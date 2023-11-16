using UnityEngine;

public class DamageOnCollision : MonoBehaviour
{
    [SerializeField] float damage = 5f;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out IDamagable damagable))
        {
            damagable.TakeDamage(damage);
        }
    }
}