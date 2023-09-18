namespace ArkanoidGameProject;

public class GameStats
{
    public int Score { get; set; }
    public int Lives { get; set; }
    public TimeSpan Time { get; set; }

    public GameStats(int score, int lives, TimeSpan time)
    {
        Score = score;
        Lives = lives;
        Time = time;
    }
    
    public GameStats()
    {
        Score = 0;
        Lives = 3;
        Time = TimeSpan.Zero;
    }

    public event Action<int>? ScoreChanged;
    public event Action<int>? LivesChanged;
    public event Action<TimeSpan>? TimeChanged;

    public void AddScore(int score)
    {
        Score += score;
        ScoreChanged?.Invoke(Score);
    }

    public void RemoveLife()
    {
        Lives--;
        LivesChanged?.Invoke(Lives);
    }

    public void AddLife()
    {
        Lives++;
        LivesChanged?.Invoke(Lives);
    }

    public void AddTime(float time)
    {
        Time += TimeSpan.FromSeconds(time);
        TimeChanged?.Invoke(Time);
    }
}