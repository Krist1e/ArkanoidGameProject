using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace ArkanoidGameProject;

public static class Vectpr2FExtensions
{
    public static Vector2f Scale(this Vector2f vector, Vector2f scale)
    {
        return new Vector2f(vector.X * scale.X, vector.Y * scale.Y);
    }
    
    public static float Dot(this Vector2f vector, Vector2f other)
    {
        return vector.X * other.X + vector.Y * other.Y;
    }
    
    public static float Length(this Vector2f vector)
    {
        return (float)Math.Sqrt(vector.Dot(vector));
    }
    
    public static Vector2f Rotate(this Vector2f vector, float angle)
    {
        var cos = (float)Math.Cos(angle);
        var sin = (float)Math.Sin(angle);
        return new Vector2f(vector.X * cos - vector.Y * sin, vector.X * sin + vector.Y * cos);
    }
}