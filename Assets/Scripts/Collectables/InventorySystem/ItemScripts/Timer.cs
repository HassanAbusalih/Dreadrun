using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    float timer = 0;
    float duration = 0;
    bool timerStarts;

    public Action onTimerMet;
    [SerializeField] SoundSO finishedTimerSFX;
    AudioSource audioSource;

    public void SetTimerAndDuration(float timer, float duration)
    {
        this.timer = timer;
        this.duration = duration;
        timerStarts = true;
    }
    private void Update()
    {
        TimerSet();
    }

    void TimerSet()
    {
        timer += Time.deltaTime;
        if (timer > duration && timerStarts)
        {
            onTimerMet?.Invoke();
            timerStarts = false;
            finishedTimerSFX.PlaySound(ref audioSource, 0, this.gameObject, false, false);
            Destroy(gameObject);
        }
    }
}
