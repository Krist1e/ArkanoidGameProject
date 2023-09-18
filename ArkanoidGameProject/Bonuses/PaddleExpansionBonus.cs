using SFML.Graphics;
namespace ArkanoidGameProject.Bonuses;

public sealed class PaddleExpansionBonus : Bonus
{
    public PaddleExpansionBonus()
    {
    }

    public PaddleExpansionBonus(TimeSpan duration)
    {
        Duration = duration;
    }

    public override Color Color => Color.Magenta;
    public override TimeSpan Duration { get; protected set; } = TimeSpan.FromSeconds(10);

    public override void ApplyEffect(IPlayerProperties playerProperties)
    {
        playerProperties.PaddleSize = playerProperties.PaddleSize with { X = playerProperties.PaddleSize.X * 2 };
    }

    public override void RemoveEffect(IPlayerProperties playerProperties)
    {
        playerProperties.PaddleSize = playerProperties.PaddleSize with { X = playerProperties.PaddleSize.X / 2 };
    }
}
