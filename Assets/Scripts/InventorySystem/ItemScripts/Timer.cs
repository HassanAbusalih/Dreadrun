using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    float timer=0;
    float duration=0;
    bool timerSettingSet;

    public Action onTimerMet;

    public void SetTimerAndDuration(float timer, float duration)
    {
        this.timer = timer; 
        this.duration = duration;
        timerSettingSet = true;
    }
    private void Update()
    {
        TimerMet();
    }

    void TimerMet()
    {
        timer += Time.deltaTime;
        if(timer > duration && timerSettingSet)
        {
            onTimerMet?.Invoke();
            timerSettingSet = false;
            Destroy(gameObject);
        }
    }
}
