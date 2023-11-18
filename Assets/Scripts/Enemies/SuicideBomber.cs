using UnityEditor;
using UnityEngine;

public class SuicideBomber : EnemyAIBase
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
    LayerMask mask;

    private void Start()
    {
        lastAttackTime -= attackCooldown;
        mask = ~(LayerMask.GetMask("Enemy Projectile"));
    }

    private void Update()
    {
        Transform closestPlayer = GetClosestPlayer();
        if (closestPlayer == null || !LineOfSight(closestPlayer) || Time.time <= attackCooldown + lastAttackTime)
        {
            rb.velocity = new(0f, rb.velocity.y, 0f);
            return;
        }
    }

    bool LineOfSight(Transform target)
    {
        RaycastHit hit;
        Vector3 directionToTarget = target.position - transform.position;
        if (Physics.Raycast(transform.position, directionToTarget, out hit, detectionRange, mask))
        {
            return hit.transform == target;
        }
        return false;
    }

    public override void Move(Transform target, float movementSpeed)
    {
        Vector3 moveDirection = (target.position - transform.position).normalized;
        moveDirection = moveDirection.normalized * movementSpeed;
        moveDirection.y = 0;
        transform.rotation = Quaternion.LookRotation(moveDirection);
        rb.velocity = new(moveDirection.x, rb.velocity.y, moveDirection.z);
    }

    void StopAttack()
    {
        if (chargeAndAttack != null)
        {
            StopCoroutine(chargeAndAttack);
            chargeAndAttack = null;
        }
        lastAttackTime = Time.time;
    }

    private void OnDisable()
    {
        if (weapon != null)
        {
            (weapon as MeleeFodderWeapon).hit -= StopAttack;
        }
    }

    private void OnEnable()
    {
        weapon = GetComponentInChildren<MeleeFodderWeapon>();
        if (weapon != null)
        {
            (weapon as MeleeFodderWeapon).hit += StopAttack;
        }
    }

    private void OnDrawGizmos()
    {
#if UNITY_EDITOR
        Handles.color = new Color(0, 0, 1, 0.2f);
        Handles.DrawSolidDisc(transform.position, Vector3.up, attackRange);
        Handles.color = new Color(1, 0, 0, 0.1f);
        Handles.DrawSolidDisc(transform.position, Vector3.up, detectionRange);
#endif
    }
}