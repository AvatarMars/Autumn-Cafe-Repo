using System;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [field: SerializeField, Min(0f)] public float MaxTime { get; set; } = 3f;

    [field: SerializeField, Min(-1f)] public float DefaultTime { get; set; } = -1f;
    
    [field: SerializeField] public float CurrentTime { get; private set; }
    
    [field: Tooltip("If true, time will flow descending (from MaxTime to 0), otherwise it'll flow ascending")]
    [field: SerializeField] public bool IsTimeInverted { get; set; }
    
    [field: SerializeField] public bool IsRunning { get; set; }

    public Action onTimerTickFinished;

    private float _originalMaxTime;

    // Start is called before the first frame update
    void Start()
    {
        _originalMaxTime = MaxTime;
    }

    // Update is called once per frame
    void Update()
    {
        Tick();
    }

    public void StartTimer()
    {
        ResetTimer();
        ResumeTime();
    }

    public void StopTimer() => IsRunning = false;

    public void ResumeTime() => IsRunning = true;

    private void Tick()
    {
        if (!IsRunning) return;

        var deltaTime = Time.deltaTime;
        CurrentTime += IsTimeInverted ? -deltaTime : deltaTime;

        if (CurrentTime > MaxTime || CurrentTime < 0) OnTimerTickFinished();
    }

    private void OnTimerTickFinished()
    {
        onTimerTickFinished?.Invoke();
        IsRunning = false;
    }

    public void ResetTimer()
    {
        if (IsTimeInverted)
        {
            CurrentTime = DefaultTime >= 0 ? DefaultTime : MaxTime;
        }
        else
        {
            CurrentTime = DefaultTime >= 0 ? DefaultTime : 0;
        }
    }

    public void ResetMaxTime()
    {
        MaxTime = _originalMaxTime;
        if (IsTimeInverted)
        {
            if (CurrentTime > MaxTime) CurrentTime = MaxTime; 
        }
    }
}
