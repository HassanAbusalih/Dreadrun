using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PayloadCheckpointSystem : MonoBehaviour
{
    //private PlayerStats[] playerStatsArray;
    //private ObjectSpawner[] enemySpawners;
    private PayloadStats payloadStats;

    [NonSerialized]
    public bool onCheckpoint;

    [SerializeField]
    private float checkpointDuration;

    private void Start()
    {
        //playerStatsArray = FindObjectsOfType<PlayerStats>();
        //enemySpawners = FindObjectsOfType<ObjectSpawner>();
        payloadStats = FindObjectOfType<PayloadStats>();
    }

    public IEnumerator ActivateCheckpoint()
    {
        onCheckpoint = true;
        //foreach(PlayerStats playerstats in playerStatsArray)
        //{
        //    playerstats.EXP += payloadStats.storedEXP / playerStatsArray.Count();
        //}

        //foreach(ObjectSpawner spawner in enemySpawners)
        //{
        //    spawner.enabled = false();
        //}

        payloadStats.storedEXP = 0;

        yield return new WaitForSeconds(checkpointDuration);

        DeactivateCheckpoint();
    }

    private void DeactivateCheckpoint()
    {
        onCheckpoint = false;

        //foreach(ObjectSpawner spawner in enemySpawners)
        //{
        //    spawner.enabled = false();
        //}
    }
}
