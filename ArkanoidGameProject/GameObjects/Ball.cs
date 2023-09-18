using SFML.Graphics;
using SFML.System;
namespace ArkanoidGameProject.GameObjects
{
    public class Ball : DisplayObject
    {
        private readonly CircleShape _shape;
        private float _radius;

        private Color _color;

        public Ball(float radius, Vector2f position, Vector2f velocity)
        {
            _shape = new CircleShape(radius);               
            Radius = radius;
            Color = Color.Yellow;
            Position = position;
            Velocity = velocity;
        }
        
        public Ball()
        {
            _shape = new CircleShape();
        }

        public Vector2f Velocity { get; set; }

        public Color Color
        {
            get => _color;
            set
            {
                _color = value;
                _shape.FillColor = _color;
            }
        }

        public float Radius
        {
            get => _radius;
            set
            {
                _radius = value;
                _shape.Origin = new Vector2f(_radius, _radius);
                _shape.Radius = _radius;
                Size = new Vector2f(_radius * 2, _radius * 2);
            }
        }

        public override void Draw(RenderWindow window)
        {
            window.Draw(_shape);
        }

        public override void Move(float deltaTime)
        {
            Position += Velocity * deltaTime;
        }

        public override void OnCollision(DisplayObject obj, CollisionArgs args)
        {
            switch (obj)
            {
                case Block block:
                    block.TakeDamage();
                    Bounce(args.Normal, args.Penetration);
                    break;
                case Paddle paddle:
                    Bounce(args.Normal, args.Penetration, paddle.Position, paddle.Size.X);
                    break;
                default:
                    Bounce(args.Normal, args.Penetration);
                    break;
            }
        }

        public override void OnWallCollision(CollisionArgs args)
        {
            if (args.Normal != new Vector2f(0, -1))
                Bounce(args.Normal, args.Penetration);
            else
                Destroy();
        }

        private void Bounce(Vector2f normal, float penetration, Vector2f paddlePosition = default, float paddleWidth = 0)
        {
            var normalPerpendicular = new Vector2f(normal.Y, -normal.X);
            var velocityDirection = Velocity / Velocity.Length();
            var velocityNormal = velocityDirection.Dot(normal);
            var velocityTangent = velocityDirection.Dot(normalPerpendicular);
            var velocityAfterBounce = -velocityNormal * normal + velocityTangent * normalPerpendicular;
            Velocity = velocityAfterBounce / velocityAfterBounce.Length() * Velocity.Length();
            Position += normal * penetration;

            if (paddleWidth == 0) return;
            
            var distanceFromCenter = Position.X - paddlePosition.X;
            var normalizedDistance = distanceFromCenter / paddleWidth;
            var angle = MathF.PI / 4 * normalizedDistance;
            Velocity = Velocity.Rotate(angle);
        }

        protected override void OnPositionChanged()
        {
            base.OnPositionChanged();
            _shape.Position = Position;
        }
    }
}
