using SFML.Graphics;
namespace ArkanoidGameProject.Bonuses;

public abstract class Bonus
{
    public abstract Color Color { get; }
    public abstract TimeSpan Duration { get; protected set; }

    public abstract void ApplyEffect(IPlayerProperties playerProperties);
    public abstract void RemoveEffect(IPlayerProperties playerProperties);

    public void UpdateTime(float deltaTime)
    {
        Duration -= TimeSpan.FromSeconds(deltaTime);
    }
}
