using System.Collections;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(FlockingBehavior))]
public class MeleeFodderEnemy : EnemyAIBase
{
    [SerializeField] float detectionRange = 10;
    [SerializeField] float attackRange = 2.0f;
    [SerializeField] float attackCooldown = 1.5f;
    [SerializeField] float dashSpeedModifier = 5f;
    [SerializeField] float chargeSpeedModifier = 0.5f;
    [SerializeField] float chargeTime = 1f;
    [SerializeField] float dashTime = 0.2f;
    float lastAttackTime;
    Coroutine chargeAndAttack;

    private void Start()
    {
        lastAttackTime -= attackCooldown;
    }

    private void Update()
    {
        Transform closestPlayer = GetClosestPlayer();
        if (closestPlayer == null || !LineOfSight(closestPlayer) || Time.time <= attackCooldown + lastAttackTime)
        {
            rb.velocity = new(0f, rb.velocity.y, 0f);
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, closestPlayer.position);
        if (distanceToPlayer <= detectionRange && distanceToPlayer > attackRange)
        {
            Move(closestPlayer, movementSpeed);
        }
        else if (distanceToPlayer <= attackRange && chargeAndAttack == null)
        {
            chargeAndAttack = StartCoroutine(ChargeAndAttack(closestPlayer));
        }
    }

    bool LineOfSight(Transform target)
    {
        RaycastHit hit;
        Vector3 directionToTarget = target.position - transform.position;
        if (Physics.Raycast(transform.position, directionToTarget, out hit, detectionRange))
        {
            return hit.transform == target;
        }
        return false;
    }

    public override void Move(Transform target, float movementSpeed)
    {
        Vector3 moveDirection = (target.position - transform.position).normalized + flockingBehavior.Flocking(movementSpeed);
        moveDirection = moveDirection.normalized * movementSpeed;
        moveDirection.y = 0;
        transform.rotation = Quaternion.LookRotation(moveDirection);
        rb.velocity = new(moveDirection.x, rb.velocity.y, moveDirection.z);
    }

    IEnumerator ChargeAndAttack(Transform player)
    {
        float startTime = Time.time;
        Debug.Log("Charging");
        while (Time.time < startTime + chargeTime)
        {
            Move(player, movementSpeed * chargeSpeedModifier);
            yield return null;
        }

        startTime = Time.time;
        Debug.Log("Dashing");
        while (Time.time < startTime + dashTime)
        {
            Move(player, dashSpeedModifier);
            yield return null;
        }

        Debug.Log("Finished");
        lastAttackTime = Time.time;
        yield return new WaitForSeconds(attackCooldown);

        Debug.Log("Ready to go again!");
        chargeAndAttack = null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = new Color(0, 0, 0, 0.2f);
        Gizmos.DrawSphere(transform.position, attackRange);
        Handles.color = new Color(1, 0, 0, 0.2f);
        Handles.DrawSolidDisc(transform.position, Vector3.up, attackRange);
    }
}