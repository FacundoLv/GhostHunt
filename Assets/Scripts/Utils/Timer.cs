using System;

public class Timer
{
    public float Countdown { get => _countDownTime; }

    public event Action OnTimerComplete;

    private float _countDownTime;

    public Timer (float countDownTime)
    {
        _countDownTime = countDownTime;
    }

    public void Tick(float deltaTime)
    {
        _countDownTime -= deltaTime;

        if (_countDownTime <= 0f)
        {
            _countDownTime = 0f;
            OnTimerComplete?.Invoke();
        }
    }
}
