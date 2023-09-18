using ArkanoidGameProject.UI.Components;
using SFML.Graphics;
using SFML.System;
namespace ArkanoidGameProject.GameObjects;

public class PowerUpMessage : DisplayObject
{
    private readonly Color _color;
    private readonly TextLabel _textLabel;
    private float _timeToLive = 2f;
    
    public PowerUpMessage(TextLabel textLabel)
    {
        _textLabel = textLabel;
        _color = textLabel.Color;
        IsStatic = false;
    }

    public override void Draw(RenderWindow window)
    {
        _textLabel.Draw(window);
    }
    public override void OnWallCollision(CollisionArgs args)
    {
    }
    public override void OnCollision(DisplayObject obj, CollisionArgs args)
    {
    }
    public override void Move(float deltaTime)
    {
        _timeToLive -= deltaTime;
        _textLabel.Position -= new Vector2f(0, -deltaTime * 50);
        _textLabel.Color = new Color(_color.R, _color.G, _color.B, (byte) (255 * _timeToLive / 2));
        if (_timeToLive <= 0)
            Destroy();
    }
}
