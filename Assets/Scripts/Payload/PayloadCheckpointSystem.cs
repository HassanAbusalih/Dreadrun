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
    PayloadMovement payloadMovement;
    ObjectSpawner[] enemySpawners;

    private void Start()
    {
        enemySpawners = FindObjectsOfType<ObjectSpawner>();
        checkpointTimer = checkpointDuration;
        checkpointUI.enabled = false;
        playersInGame = FindObjectsOfType<Player>();
        payloadMovement = FindObjectOfType<PayloadMovement>();
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

        payloadMovement.movementEnabled = false;

        foreach (Player player in playersInGame)
        {
            player.gameObject.GetComponent<PlayerExp>().LevelUp();
        }

        foreach (ObjectSpawner spawner in enemySpawners)
        {
            spawner.enabled = false;
        }

        yield return new WaitForSeconds(checkpointDuration);

        DeactivateCheckpoint();
    }

    private void DeactivateCheckpoint()
    {
        checkpointUI.enabled = false;
        onCheckpoint = false;

        payloadMovement.movementEnabled = true;

        foreach (ObjectSpawner spawner in enemySpawners)
        {
            spawner.enabled = true;
        }
    }
}
