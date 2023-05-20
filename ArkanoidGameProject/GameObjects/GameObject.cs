using SFML.Graphics;
using SFML.System;

namespace ArkanoidGameProject.GameObjects
{
    public abstract class GameObject
    {      
        public Vector2f Position { get; set; }
        public Vector2f Size { get; set; }
        public Vector2f LeftTop => Position - Size / 2;
        public Vector2f RightBottom => Position + Size / 2;
        public bool IsEnabled { get; set; } = true;
        public bool IsVisible { get; set; } = true;
        public bool IsStatic { get; set; } = false;       

        public Action<GameObject>? Destroyed;
        public abstract void Draw(RenderWindow window);
        public abstract void OnWallCollision(CollisionArgs args);
        public abstract void OnCollision(GameObject obj, CollisionArgs args);

        public bool CheckCollision(GameObject other, out CollisionArgs? args)
        {
            args = default;

            if (RightBottom.X < other.LeftTop.X || LeftTop.X > other.RightBottom.X)
                return false;

            if (RightBottom.Y < other.LeftTop.Y || LeftTop.Y > other.RightBottom.Y)
                return false;

            var overlapX = MathF.Min(RightBottom.X - other.LeftTop.X, other.RightBottom.X - LeftTop.X);
            var overlapY = MathF.Min(RightBottom.Y - other.LeftTop.Y, other.RightBottom.Y - LeftTop.Y);

            Vector2f normal;
            float penetration;
            
            if (overlapX > overlapY)
            {
                normal = new Vector2f(0, MathF.Sign(LeftTop.Y - other.LeftTop.Y));
                penetration = overlapY;
            }
            else
            {
                normal = new Vector2f(MathF.Sign(LeftTop.X - other.LeftTop.X), 0);
                penetration = overlapX;
            }

            args = new CollisionArgs(normal, penetration);
            return true;
        }

        public abstract void Move(float deltaTime);
        public void Destroy()
        {
            Destroyed?.Invoke(this);
        }
    }

    public class CollisionArgs
    {
        public Vector2f Normal { get; set; } = new Vector2f(0.0f, 0.0f);
        public float Penetration { get; set; } = 0;
        public CollisionArgs(Vector2f normal, float penetration)
        {
            Normal = normal;
            Penetration = penetration;
        }
        public void Reflect()
        {
            Normal = -Normal;
        }
    }
}


