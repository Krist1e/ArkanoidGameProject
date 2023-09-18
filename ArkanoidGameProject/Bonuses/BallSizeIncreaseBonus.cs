using SFML.Graphics;
namespace ArkanoidGameProject.Bonuses;

public sealed class BallSizeIncreaseBonus : Bonus
{
    public BallSizeIncreaseBonus()
    {
    }

    public BallSizeIncreaseBonus(TimeSpan duration)
    {
        Duration = duration;
    }

    public override Color Color => Color.Cyan;
    public override TimeSpan Duration { get; protected set; } = TimeSpan.FromSeconds(10);

    public override void ApplyEffect(IPlayerProperties playerProperties)
    {
        playerProperties.BallsRadius *= 2;
    }

    public override void RemoveEffect(IPlayerProperties playerProperties)
    {
        playerProperties.BallsRadius /= 2;
    }
}
