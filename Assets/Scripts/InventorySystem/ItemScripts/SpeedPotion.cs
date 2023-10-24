using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "New Potion", menuName = "Inventory/Speed Potion")]
public class SpeedPotion : ItemBase
{
    [SerializeField] int speedIncrease;
    float defaultSpeed;
    float timer = 0;
    [SerializeField] float duration;
    [SerializeField] GameObject outsideTimerPrefab;
    [SerializeField] Player playerRef;

    public override void UseOnSelf(Player player)
    {
        hasBuffedItem = true;
        defaultSpeed = player.playerStats.speed;
        player.playerStats.speed = speedIncrease;

        Debug.Log("Speed increased");
        GetTimer();
        playerRef = player;

    }

    private void GetTimer()
    {
        Timer _outsideTimer = Instantiate(outsideTimerPrefab).GetComponent<Timer>();
        _outsideTimer.SetTimerAndDuration(timer, duration);
        _outsideTimer.onTimerMet += ResetStats;
    }

    private void ResetStats()
    {
        playerRef.playerStats.speed = defaultSpeed;
        hasBuffedItem = false;
        Debug.Log("Speed back to normal");
    }

}
