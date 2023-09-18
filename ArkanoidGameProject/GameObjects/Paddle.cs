using ArkanoidGameProject.Input;
using SFML.Graphics;
using SFML.System;
using SFML.Window;


namespace ArkanoidGameProject.GameObjects
{
    public class Paddle : DisplayObject
    {
        private readonly RectangleShape _shape;
        private Color _color;
        private Ball? _ball;
        private Vector2f _velocity;

        public Paddle(Vector2f position, float width, float height, Color color, float speed)
        {
            _shape = new RectangleShape(new Vector2f(width, height));
            Size = new Vector2f(width, height);
            Position = position;
            Color = color;
            Speed = speed;
        }

        public Paddle()
        {
            _shape = new RectangleShape();
        }

        public float Speed { get; set; }

        public Ball? Ball
        {
            get => _ball;
            set
            {
                _ball = value;
                if (_ball == null) return;
                BindBall();
            }
        }

        public Color Color
        {
            get => _color;
            set
            {
                _color = value;
                _shape.FillColor = _color;
            }
        }

        public override void Draw(RenderWindow window)
        {
            window.Draw(_shape);
        }


        public void MoveLeft(float amount)
        {
            _velocity += new Vector2f(-amount * Speed, 0);
        }

        public void MoveRight(float amount)
        {
            _velocity += new Vector2f(amount * Speed, 0);
        }

        public void BindBall()
        {
            if (Ball == null) return;
            Ball.IsEnabled = false;
            Ball.IsStatic = true;
            Ball.Position = Position + new Vector2f(0, -Ball.Radius - Size.Y / 2f);
        }

        public void ReleaseBall(int launchSpeed)
        {
            if (Ball == null) return;
            Ball.IsEnabled = true;
            Ball.IsStatic = false;
            Ball.Velocity = new Vector2f(0, -1) * launchSpeed;
            Ball = null;
        }
        public override void Move(float deltaTime)
        {
            Position += _velocity * deltaTime;
            _velocity = new Vector2f(0, 0);
            if (Ball != null) Ball.Position = Position + new Vector2f(0, -Ball.Radius - Size.Y / 2f);
        }

        public override void OnCollision(DisplayObject obj, CollisionArgs args)
        {

        }

        public override void OnWallCollision(CollisionArgs args)
        {
            Position += args.Normal * args.Penetration;
        }

        protected override void OnPositionChanged()
        {
            base.OnPositionChanged();
            _shape.Position = Position;
        }

        protected override void OnSizeChanged()
        {
            base.OnSizeChanged();
            _shape.Origin = Size / 2;
            _shape.Size = Size;
        }
    }
}
