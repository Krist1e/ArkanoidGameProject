using SFML.System;

namespace ArkanoidGameProject.Collision
{
    public class Bounds
    {
        private Vector2f _leftTop;
        private Vector2f _rightBottom;

        public Bounds(Vector2f position, Vector2f size)
        {
            _leftTop = position - size / 2;
            _rightBottom = position + size / 2;
        }

        public Vector2f Position
        {
            get => _leftTop + (_rightBottom - _leftTop) / 2;
            set
            {
                var size = _rightBottom - _leftTop;
                _leftTop = value - size / 2;
                _rightBottom = value + size / 2;
            }
        }

        public Vector2f LeftTop => _leftTop;
        public Vector2f RightBottom => _rightBottom;

        public CollisionArgs? CheckCollision(Bounds bounds)
        {
            var overlapX = Math.Max(0, Math.Min(_rightBottom.X, bounds._rightBottom.X) - Math.Max(_leftTop.X, bounds._leftTop.X));
            if (overlapX == 0)
                return null;

            var overlapY = Math.Max(0, Math.Min(_rightBottom.Y, bounds._rightBottom.Y) - Math.Max(_leftTop.Y, bounds._leftTop.Y));
            if (overlapY == 0)
                return null;

            var normal = overlapX > overlapY
                ? new Vector2f(0, Math.Sign(_leftTop.Y - bounds._leftTop.Y))
                : new Vector2f(Math.Sign(_leftTop.X - bounds._leftTop.X), 0);

            var penetration = overlapX > overlapY ? overlapY : overlapX;

            return new CollisionArgs(normal, penetration);
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
