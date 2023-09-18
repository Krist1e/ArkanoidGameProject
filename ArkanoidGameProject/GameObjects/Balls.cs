using System.Collections;
using ArkanoidGameProject.GameObjects;

namespace Arkanoid.Player;

public sealed class Balls : IEnumerable<Ball>
{
    private readonly List<Ball> _balls = new();
    private float _radius = 10;

    public int Count => _balls.Count;

    public float Radius
    {
        get => _radius;
        set
        {
            _radius = value;
            foreach (var ball in _balls) ball.Radius = _radius;
        }
    }

    public IEnumerator<Ball> GetEnumerator()
    {
        return _balls.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public event Action<Ball>? Added;
    public event Action<Ball>? Removed;

    public void Add(Ball ball)
    {
        _balls.Add(ball);
        ball.Destroyed += OnBallDestroyed;
        Added?.Invoke(ball);
    }

    public void Remove(Ball ball)
    {
        _balls.Remove(ball);
        ball.Destroyed -= OnBallDestroyed;
        Removed?.Invoke(ball);
    }

    public void Clear()
    {
        _balls.Clear();
        Added = null;
        Removed = null;
    }

    private void OnBallDestroyed(DisplayObject ball)
    {
        Remove((ball as Ball)!);
    }
}