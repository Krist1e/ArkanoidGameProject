using SFML.Graphics;
using SFML.System;

namespace ArkanoidGameProject.GameObjects
{
    public abstract class DisplayObject
    {      
        private Vector2f _position;
        private Vector2f _size;
        
        private bool _isEnabled = true;

        public Vector2f Position
        {
            get => _position;
            set
            {
                _position = value;
                OnPositionChanged();
            }
        }
        public Vector2f Size
        {
            get => _size;
            set
            {
                _size = value;
                OnSizeChanged();
            }
        }
        
        public Vector2f LeftTop
        {
            get => Position - Size / 2;
            set => Position = value + Size / 2;
        }
        public Vector2f RightBottom
        {
            get => Position + Size / 2;
            set => Position = value - Size / 2;
        }

        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                _isEnabled = value;
                OnEnabledChanged();
            }
        }
        public bool IsVisible { get; set; } = true;
        public bool IsStatic { get; set; } = false;     
        
        public int ZIndex { get; set; }

        public Action<DisplayObject>? Destroyed;
        public abstract void Draw(RenderWindow window);
        public abstract void OnWallCollision(CollisionArgs args);
        public abstract void OnCollision(DisplayObject obj, CollisionArgs args);

        public bool CheckCollision(DisplayObject other, out CollisionArgs args)
        {
            args = default;

            var overlapX = MathF.Max(0,
                MathF.Min(RightBottom.X, other.RightBottom.X) - MathF.Max(LeftTop.X, other.LeftTop.X));
            if (overlapX == 0) return false;

            var overlapY = MathF.Max(0,
                MathF.Min(RightBottom.Y, other.RightBottom.Y) - MathF.Max(LeftTop.Y, other.LeftTop.Y));
            if (overlapY == 0) return false;

            var normal = overlapX >= overlapY
                ? new Vector2f(0, MathF.Sign(Position.Y - other.Position.Y))
                : new Vector2f(MathF.Sign(Position.X - other.Position.X), 0);

            var penetrationDepth = overlapX > overlapY ? overlapY : overlapX;

            args = new CollisionArgs(normal, penetrationDepth);
            return true;
        }

        public abstract void Move(float deltaTime);

        protected virtual void OnPositionChanged() { }
        protected virtual void OnSizeChanged() { }

        protected virtual void OnEnabledChanged() { }

        public virtual void Destroy()
        {
            IsEnabled = false;
            Destroyed?.Invoke(this);
        }
    }

    public struct CollisionArgs
    {
        public Vector2f Normal { get; set; }
        public float Penetration { get; set; }
        public CollisionArgs(Vector2f normal, float penetration)
        {
            Normal = normal;
            Penetration = penetration;
        }
        public CollisionArgs Reflect()
        {
            Normal = -Normal;
            return this;
        }
    }
}


