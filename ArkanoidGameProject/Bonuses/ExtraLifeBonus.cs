using SFML.Graphics;
namespace ArkanoidGameProject.Bonuses;

public sealed class ExtraLifeBonus : Bonus
{
    public ExtraLifeBonus()
    {
    }

    public ExtraLifeBonus(TimeSpan duration)
    {
        Duration = duration;
    }

    public override Color Color => Color.Green;
    public override TimeSpan Duration { get; protected set; } = TimeSpan.FromSeconds(0);

    public override void ApplyEffect(IPlayerProperties playerProperties)
    {
        playerProperties.AddLife();
    }

    public override void RemoveEffect(IPlayerProperties playerProperties)
    {
    }
}
