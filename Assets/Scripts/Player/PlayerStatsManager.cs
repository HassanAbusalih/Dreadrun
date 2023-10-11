using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsManager : MonoBehaviour
{
    private PlayerStats baseStats;
    public PlayerStats currentStats;
    private void Start()
    {

        currentStats = new PlayerStats();
        currentStats = baseStats;

    }

    public void ApplyStatChanges(PlayerStats changes)
    {
        // Apply changes to the current stats
        currentStats.health += changes.health;
        currentStats.attack += changes.attack;
        currentStats.stamina += changes.stamina;
 
    }
}
