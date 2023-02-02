using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using ExtEvents;

public class CountDownTimer : MonoBehaviour
{
    [SerializeField] public ExtEvent onTimeout;

    public enum TimerProcessMode
    {
        TIMER_PROCESS_PHYSICS,
        TIMER_PROCESS_IDLE
    }
    [SerializeField] float waitTime = 1.0f;
    [SerializeField] bool autostart = false;
    [SerializeField] bool oneShot = true;
    [SerializeField] bool paused = false;
    [SerializeField] float timeLeft = 0;
    [SerializeField] bool isStopped = false;
    [SerializeField] public float timeMultiplier { set; get; } = 1;
    
    TimerProcessMode processMode = TimerProcessMode.TIMER_PROCESS_IDLE;
    private void OnEnable()
    {
        if (autostart)
        {
            StartTimer();
        } else
        {
            isStopped = true;
        }
    }

    private void Update()
    {
        if (processMode == TimerProcessMode.TIMER_PROCESS_IDLE)
        {
            UpdateTimer();
        }
    }
    private void FixedUpdate()
    {
        if (processMode == TimerProcessMode.TIMER_PROCESS_PHYSICS)
        {
            UpdateTimer();
        }
    }

    private void UpdateTimer()
    {
        if (!isStopped)
        {
            if (timeLeft > 0 && !paused)
            {
                timeLeft -= Time.deltaTime * timeMultiplier;
            }
            else if (timeLeft <= 0)
            {
                isStopped = true;
                timeLeft = 0;
                onTimeout?.Invoke();
                //Debug.Log(gameObject.name + " timed out");
                // Loop if one shot is false
                Loop();
            }
        }
    }

    private void Loop()
    {
        if (!oneShot)
        {
            Debug.Log("Looped timer");
            StartTimer();
        }
    }
    public bool IsStopped()
    {
        return isStopped;
    }

    public void StartTimer(float timeSec = -1)
    {
        if (timeSec > 0)
        {
            waitTime = timeSec;
        }
        timeLeft = waitTime;
        isStopped = false;
    }

    public void Stop()
    {
        isStopped = true;
        timeLeft = 0;
    }

    // Setters and getters
    public void SetOneShot(bool value)
    {
        oneShot = value;
    }

    public bool IsOneShot()
    {
        return oneShot;
    }

    public void SetPaused(bool value)
    {
        paused = value;
    }

    public bool IsPaused()
    {
        return paused;
    }

    public void SetTimerProcessMode(TimerProcessMode value)
    {
        processMode = value;
    }

    public TimerProcessMode GetTimerProcessMode()
    {
        return processMode;
    }

    public float GetTimeLeft()
    {
        return timeLeft;
    }

    public void SetWaitTime(float value)
    {
        waitTime = value;
    }

    public float GetWaitTime()
    {
        return waitTime;
    }

}
