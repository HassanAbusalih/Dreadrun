using UnityEngine;

public class TurretEnemy : EnemyAIBase
{
    [SerializeField] LayerMask layersToIgnore;
    private void Update()
    {
        Transform target = GetClosestPlayer();
        if (target != null) 
        {
            Vector3 lookDirection = (target.position - transform.position).normalized;
            lookDirection.y = 0;
            transform.rotation = Quaternion.LookRotation(lookDirection);
            weapon.Attack(); 
        }
    }

    protected override Transform GetClosestPlayer()
    {
        Transform closestPlayer = null;
        float closestDistance = float.MaxValue;
        RaycastHit hit;
        foreach (Transform player in players)
        {
            float distance = Vector3.Distance(transform.position, player.position);
            if (distance < closestDistance && 
                Physics.Raycast(transform.position, player.position - transform.position, out hit, weapon.ProjectileRange, ~layersToIgnore) && 
                hit.transform == player)
            {
                closestPlayer = player;
                closestDistance = distance;
            }
        }
        return closestPlayer;
        
    }
}
