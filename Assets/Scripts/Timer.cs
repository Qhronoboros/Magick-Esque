using System;

public class Timer
{
    private bool _looping = false;
    public bool isCounting = false;
    
    public float timerDuration = 0.0f;
    public float elapsedTime;
    
	public event Action OnTimerStart;
	public event Action OnTimerEnd;
	public event Action OnTimerStopped;
	
	public Timer(float duration = 0.0f)
	{
	    this.timerDuration = duration;
	}
	
    public void StartTimer()
    {
        if (isCounting)
            StopTimer();

        OnTimerStart?.Invoke();
        isCounting = true;
    }

    public void EnableLooping()
    {
        if (!_looping)
        {
            _looping = true;
            OnTimerEnd += StartTimer;
        }
    }
    
    public void DisableLooping()
    {
        if (_looping)
        {
            _looping = false;
            OnTimerEnd -= StartTimer;
        }
    }
    
    public void StopTimer()
    {
        ResetTimerValues();
        OnTimerStopped?.Invoke();
    }
    
    private void ResetTimerValues()
    {
        isCounting = false;
        elapsedTime = 0.0f;
    }
    
    public void CountTimer(float deltaTime)
    {
        if (!isCounting) return;
        
        elapsedTime += deltaTime;
        
        if (elapsedTime >= timerDuration)
        {
            ResetTimerValues();
            OnTimerEnd?.Invoke();
        }
    }
}
