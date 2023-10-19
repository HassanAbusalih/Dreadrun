using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PayloadCheckpointSystem : MonoBehaviour
{
    //private PlayerStats[] playerStatsArray;
    //private ObjectSpawner[] enemySpawners;
    private PayloadStats payloadStats;

    [NonSerialized]
    public bool onCheckpoint = false;
    private float checkpointTimer;
    private Player[] playersInGame;

    [SerializeField]
    private float checkpointDuration;
    [SerializeField]
    private Canvas checkpointUI;
    [SerializeField]
    private Image checkpointTimeBar;

    private void Start()
    {
        //playerStatsArray = FindObjectsOfType<PlayerStats>();
        //enemySpawners = FindObjectsOfType<ObjectSpawner>();
        payloadStats = FindObjectOfType<PayloadStats>();
        checkpointTimer = checkpointDuration;
        checkpointUI.enabled = false;
        playersInGame = FindObjectsOfType<Player>();
    }

    private void Update()
    {
        if (onCheckpoint)
        {
            checkpointTimer -= Time.deltaTime;
            checkpointTimeBar.fillAmount = checkpointTimer / checkpointDuration;
        }
    }

    public IEnumerator ActivateCheckpoint()
    {
        checkpointTimer = checkpointDuration;
        checkpointUI.enabled = true;
        onCheckpoint = true;

        foreach(Player player in playersInGame)
        {
            player.gameObject.GetComponent<PlayerExp>().LevelUp();
        }

        //foreach(ObjectSpawner spawner in enemySpawners)
        //{
        //    spawner.enabled = false;
        //}

        payloadStats.storedEXP = 0;

        yield return new WaitForSeconds(checkpointDuration);

        DeactivateCheckpoint();
    }

    private void DeactivateCheckpoint()
    {
        checkpointUI.enabled = false;
        onCheckpoint = false;

        //foreach(ObjectSpawner spawner in enemySpawners)
        //{
        //    spawner.enabled = true;
        //}
    }
}
