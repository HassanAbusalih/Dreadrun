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

        checkpointTimer = checkpointDuration;
        checkpointUI.enabled = false;
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
