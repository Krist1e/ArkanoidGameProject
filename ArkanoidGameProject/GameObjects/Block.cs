using ArkanoidGameProject.Bonuses;
using SFML.Graphics;
using SFML.System;
namespace ArkanoidGameProject.GameObjects
{
    public class Block : DisplayObject
    {
        private readonly RectangleShape _shape;
        private Color _color;
        private List<Bonus> _bonuses = new();

        public Block(Vector2f position, Vector2f size)
        {
            _shape = new RectangleShape(size);
            Size = size;
            Position = position;
            IsStatic = true;
        }
        
        public Block()
        {
            _shape = new RectangleShape();
            IsStatic = true;
        }

        public int Strength { get; set; } = 4;

        public Color Color
        {
            get => _color;
            set
            {
                _color = value;
                _shape.FillColor = _color;
            }
        }
        
        public IEnumerable<Bonus> Bonuses 
        {
            get => _bonuses;
            set
            {
                _bonuses = value.ToList();
                Color = MixColors(_bonuses);
            }
        }

        public override void Draw(RenderWindow window)
        {
            window.Draw(_shape);
        }

        public override void Move(float deltaTime)
        {
        }

        public override void OnCollision(DisplayObject obj, CollisionArgs args)
        {
        }

        public override void OnWallCollision(CollisionArgs args)
        {
        }

        public void TakeDamage()
        {
            if (--Strength <= 0)
                Destroy();

            Color = new Color(Color.R, Color.G, Color.B, (byte)(Strength * 40));
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
        
        private Color MixColors(IEnumerable<Bonus> bonuses)
        {
            return bonuses.Aggregate(Color.White, (current, bonus) => current * bonus.Color);
        }
    }
}
