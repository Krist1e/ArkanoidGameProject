using SFML.Graphics;
using SFML.System;

namespace ArkanoidGameProject.GameObjects
{
    class Ball : GameObject
    {
        private float _radius;
        public float Radius
        {
            get => _radius;
            set
            {
                _radius = value;
                Shape.Origin = new Vector2f(_radius, _radius);
                Shape.Radius = _radius;
                Size = new Vector2f(_radius * 2, _radius * 2);
            }
        }
        
        public new Vector2f Position
        {
            get => base.Position;
            set
            {
                base.Position = value;
                Shape.Position = value;
            }
        }
        public Vector2f Velocity { get; set; }
        public CircleShape Shape { get; set; }
        public Color Color
        {
            get
            {
                return _color;
            }
            set
            {
                _color = value;
                Shape.FillColor = _color;
            }
        }

        private Color _color;
        public Ball(float radius, Vector2f position, Vector2f velocity)
        {
            Shape = new CircleShape(radius);               
            Radius = radius;
            Color = Color.Yellow;
            Position = position;
            Velocity = velocity;
        }

        public override void Draw(RenderWindow window)
        {
            window.Draw(Shape);
        }

        public override void Move(float deltaTime)
        {
            Position += Velocity * deltaTime;
        }

        public override void OnCollision(GameObject obj, CollisionArgs? args)
        {
            switch (obj)
            {
                case Block block:
                    block.TakeDamage();
                    break;

            }
            Bounce(args.Normal, args.Penetration);
        }

        public override void OnWallCollision(CollisionArgs? args)
        {
            Bounce(args.Normal, args.Penetration);
        }

        private void Bounce(Vector2f normal, float penetration = 0)
        {
            var dot = Velocity.X * normal.X + Velocity.Y * normal.Y;
            var bounce = normal * dot * 2;
            Velocity -= bounce;
            Random random = new();
            Velocity += new Vector2f(random.Next(-3, 3), 0);
            Position += normal * penetration;
        }
    }
}
