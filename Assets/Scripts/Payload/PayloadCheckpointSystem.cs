using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PayloadCheckpointSystem : MonoBehaviour
{
    [NonSerialized] public bool onCheckpoint = false;
    [SerializeField] float checkpointDuration;
    float checkpointTimer;

    [SerializeField] Canvas checkpointUI;
    [SerializeField] Image checkpointTimeBar;

    Player[] playersInGame;
    PayloadStats payloadStats;
    ObjectSpawner[] enemySpawners;

   

    private void Start()
    {
        enemySpawners = FindObjectsOfType<ObjectSpawner>();
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

        foreach (Player player in playersInGame)
        {
            player.gameObject.GetComponent<PlayerExp>().LevelUp();
        }

        foreach (ObjectSpawner spawner in enemySpawners)
        {
            spawner.enabled = false;
        }

        payloadStats.storedEXP = 0;

        yield return new WaitForSeconds(checkpointDuration);

        DeactivateCheckpoint();
    }

    private void DeactivateCheckpoint()
    {
        checkpointUI.enabled = false;
        onCheckpoint = false;

        foreach (ObjectSpawner spawner in enemySpawners)
        {
            spawner.enabled = true;
        }
    }
}
