using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UniJSON;

public class PayloadUITimer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI payloadTimerText;
    bool timerActivated;
    float payloadDuration;
    float liveTimer;
    Color startColor;
    private void OnEnable()
    {
        payloadTimerText.gameObject.SetActive(false);
        Payload.reachedLastCheckpoint += ActivateTimer;
        startColor = payloadTimerText.color;
    }
    private void OnDisable()
    {
        Payload.reachedLastCheckpoint -= ActivateTimer;

    }
    void FixedUpdate()
    {
        if (timerActivated && liveTimer > 0)
        {
            liveTimer -= Time.fixedDeltaTime;
            int timer = (int)liveTimer;
            payloadTimerText.text = timer.ToString();
            payloadTimerText.color = liveTimer < 10 && timer%2==0 ? Color.red : startColor;

        }
        if (liveTimer <= 0)
        { 
            payloadTimerText.gameObject.SetActive(false);
        }
    }

    void ActivateTimer(float duration)
    {
        payloadDuration = duration;
        liveTimer = payloadDuration;
        timerActivated = true;
        payloadTimerText?.gameObject.SetActive(true);
    }
}
