using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PayloadCheckpointSystem : MonoBehaviour
{
    public static PayloadCheckpointSystem Instance { get; private set; }

    [Header("Events")]
    public UnityEvent onCheckpointActivate;
    public UnityEvent onCheckpointDeactivate;

    [SerializeField] bool onCheckpoint = false;
    [SerializeField] float checkpointDuration;
    float checkpointTimer;

    [SerializeField] Canvas checkpointUI;
    [SerializeField] Image checkpointTimeBar;

    Player[] playersInGame;
    PayloadMovement payloadMovement;
    ObjectSpawner[] enemySpawners;

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

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

        onCheckpointActivate.Invoke();

        yield return new WaitForSeconds(checkpointDuration);

        onCheckpointDeactivate.Invoke();
    }
}
