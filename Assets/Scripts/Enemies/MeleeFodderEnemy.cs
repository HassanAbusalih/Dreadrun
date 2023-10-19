using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MeleeFodderEnemy : EnemyAIBase
{
    [SerializeField] float detectionRange = 10;
    [SerializeField] float movementSpeed = 5;
    [SerializeField] float damageOnHit = 5;
    Transform targetPlayer;

    private void FixedUpdate()
    {
        targetPlayer = GetClosestPlayer();
        RaycastHit hit;
        if (targetPlayer != null && Physics.Raycast(transform.position, targetPlayer.position - transform.position, out hit, detectionRange) && hit.transform == targetPlayer)
        {
            transform.LookAt(targetPlayer.position);
            Vector3 moveDirection = (targetPlayer.position - transform.position).normalized;
            rb.velocity = moveDirection * movementSpeed * Time.deltaTime;
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (targetPlayer != null && collision.transform == targetPlayer)
        {
            if (targetPlayer.TryGetComponent(out IDamagable player)) { player.TakeDamage(damageOnHit); }
        }
    }
}