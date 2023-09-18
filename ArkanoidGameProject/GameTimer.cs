using System.Diagnostics;

namespace ArkanoidGameProject;

public class GameTimer
{
    private readonly Stopwatch _stopwatch = new();
    private readonly TimeSpan _interval;
    private TimeSpan _elapsedTime;

    public GameTimer(float interval)
    {
        _interval = TimeSpan.FromSeconds(interval);
    }

    public event Action? Elapsed;
    
    public void Start()
    {
        _stopwatch.Start();
    }
    
    public void Stop()
    {
        _stopwatch.Stop();
    }

    public void Tick()
    {
        if (_stopwatch.Elapsed - _elapsedTime >= _interval)
        {
            _elapsedTime = _stopwatch.Elapsed;
            Elapsed?.Invoke();
        }
    }
}