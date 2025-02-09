using System;
using UnityEngine;

public class Timer
{
    public event Action TimerComplete;

    public float Duration { get; private set; }
    public float RemainingTime { get; private set; }
    public bool IsRunning { get; private set; }

    public Timer(float duration)
    {
        Duration = duration;
        RemainingTime = duration;
    }

    public void Restart()
    {
        IsRunning = true;
        RemainingTime = Duration;
    }

    public void Start() => IsRunning = true;

    public void Stop() => IsRunning = false;

    public void Update()
    {
        if (IsRunning == false)
            return;

        RemainingTime -= Time.deltaTime;

        if (RemainingTime <= 0)
        {
            IsRunning = false;
            RemainingTime = Duration;
            TimerComplete?.Invoke();
        }
    }
}
