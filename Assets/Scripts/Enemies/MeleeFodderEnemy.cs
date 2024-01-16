using System.Collections;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MeleeFodderEnemy : EnemyAIBase
{
    [SerializeField] float detectionRange = 10;
    [SerializeField] float attackRange = 2.0f;
    [SerializeField] float attackCooldown = 1.5f;
    [SerializeField] float dashSpeedModifier = 5f;
    [SerializeField] float chargeSpeedModifier = 0.5f;
    [SerializeField] float chargeTime = 1f;
    [SerializeField] float dashTime = 0.2f;
    [SerializeField] SoundSO dashSound;
    float lastAttackTime;
    Coroutine chargeAndAttack;
    LayerMask mask;
    GameObject trail;

    private void Start()
    {
        lastAttackTime -= attackCooldown;
        mask = ~(LayerMask.GetMask("Enemy Projectile"));
        TrailRenderer trailRenderer = GetComponentInChildren<TrailRenderer>();
        if (trailRenderer != null) 
        { 
            trail = trailRenderer.gameObject; 
            trail.SetActive(false);
        }
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
        if (Physics.Raycast(transform.position, directionToTarget, out hit, detectionRange, mask))
        {
            return hit.transform == target;
        }
        return false;
    }

    public void Charge(Vector3 forward, float movementSpeed)
    {
        RaycastHit hit;
        if (!Physics.Raycast(transform.position, transform.forward, out hit, 1f, mask))
        {
            Vector3 forwardMovement = forward * movementSpeed;

            if (Physics.Raycast(transform.position, Vector3.down, out hit))
            {
                float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
                if (slopeAngle <= 45)
                {
                    forwardMovement = Vector3.ProjectOnPlane(forwardMovement, hit.normal);
                }
                else
                {
                    // If the slope is too steep, reduce movement or stop
                    forwardMovement = Vector3.zero;
                }
            }
        
            rb.velocity = new Vector3(forwardMovement.x, rb.velocity.y, forwardMovement.z);
        }
        else
        {
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }
    }

    public override void Move(Transform target, float movementSpeed)
    {
        Vector3 moveDirection = (target.position - transform.position).normalized;
        moveDirection = moveDirection.normalized * movementSpeed;
        moveDirection.y = 0;
        transform.rotation = Quaternion.LookRotation(moveDirection);
        rb.velocity = new(moveDirection.x, rb.velocity.y, moveDirection.z);
    }

    IEnumerator ChargeAndAttack(Transform player)
    {
        float startTime = Time.time;
        while (Time.time < startTime + chargeTime)
        {
            Move(player, movementSpeed * chargeSpeedModifier);
            yield return null;
        }

        startTime = Time.time;
        (weapon as MeleeFodderWeapon).Attack(dashTime);
        if(dashSound != null) dashSound.PlaySound(0, AudioSourceType.Enemy);

        Vector3 forward = transform.forward;
        if (trail != null) { trail.SetActive(true); }
        while (Time.time < startTime + dashTime)
        {
            Charge(forward, movementSpeed * dashSpeedModifier);
            yield return null;
        }

        if (trail != null) { trail.SetActive(false); }
        lastAttackTime = Time.time;
        chargeAndAttack = null;
        rb.velocity = new(0f, rb.velocity.y, 0f);
    }

    void StopAttack()
    {
        if (chargeAndAttack != null)
        {
            StopCoroutine(chargeAndAttack);
            chargeAndAttack = null;
            if (trail != null) { trail.SetActive(false); }
            rb.velocity = Vector3.zero;
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
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Handles.color = new Color(0, 0, 1, 0.2f);
        Handles.DrawSolidDisc(transform.position, Vector3.up, attackRange);
        Handles.color = new Color(1, 0, 0, 0.1f);
        Handles.DrawSolidDisc(transform.position, Vector3.up, detectionRange);
    }
#endif
}