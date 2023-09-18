using SFML.Graphics;
namespace ArkanoidGameProject.Bonuses;

public class ExtraScoreBonus : Bonus
{
    public ExtraScoreBonus()
    {
        Score = new Random().Next(2, 10);
    }
    
    public ExtraScoreBonus(TimeSpan duration, int score)
    {
        Score = score;
    }

    public int Score { get; }
    public override Color Color => new((byte)(255 - Score * 23), (byte)(255 - Score * 23), (byte)(255 - Score * 23), 255);
    public override TimeSpan Duration { get; protected set; } = TimeSpan.FromSeconds(0);
    public override void ApplyEffect(IPlayerProperties playerProperties)
    {
        playerProperties.AddScore(Score);
    }

    public override void RemoveEffect(IPlayerProperties playerProperties)
    {
    }
}