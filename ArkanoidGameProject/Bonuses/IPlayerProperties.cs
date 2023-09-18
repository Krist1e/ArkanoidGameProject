using Arkanoid.Player;
using ArkanoidGameProject.GameObjects;
using SFML.System;
namespace ArkanoidGameProject.Bonuses;

public interface IPlayerProperties
{
    public Vector2f PaddleSize { get; set; }
    public float PaddleSpeed { get; set; }
    public float BallsRadius { get; set; }
    public void AddLife();
    public void AddScore(int score);
}
