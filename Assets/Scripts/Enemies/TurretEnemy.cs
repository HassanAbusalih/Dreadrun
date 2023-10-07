using UnityEngine;

public class TurretEnemy : EnemyAIBase
{
    private void Awake()
    {
        PlayerController[] playerControllers = FindObjectsOfType<PlayerController>();
        players = new Transform[playerControllers.Length];
        for (int i = 0; i < players.Length; i++)
        {
            players[i] = playerControllers[i].transform;
        }
    }
    private void Update()
    {
        Transform target = GetClosestPlayer();
        if (target != null) 
        {
            transform.LookAt(target);
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
            if (distance < closestDistance && Physics.Raycast(transform.position, player.position - transform.position, out hit, weapon.ProjectileRange) && hit.transform == player)
            {
                closestPlayer = player;
                closestDistance = distance;
            }
        }
        return closestPlayer;
        
    }
}
