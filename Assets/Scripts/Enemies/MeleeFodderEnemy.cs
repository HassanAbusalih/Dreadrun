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
            Vector3 moveDirection = (targetPlayer.position - transform.position).normalized * Time.deltaTime;
            rb.velocity = new Vector3(moveDirection.x,rb.velocity.y, moveDirection.z)  * movementSpeed;
        }
        else
        {
            rb.velocity = new Vector3(0,rb.velocity.y,0);
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
