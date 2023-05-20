using ArkanoidGameProject.Collision;
using SFML.Graphics;
using SFML.System;

namespace ArkanoidGameProject.GameObjects
{
    internal class Block : GameObject
    {
        private Vector2f _size;
        public Vector2f Size
        {
            get => _size;
            set
            {
                _size = value;
                Shape.Origin = _size / 2;
                Shape.Size = _size;
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
        public RectangleShape Shape { get; set; }
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
        public int Strength { get; set; } = 4;

        private Color _color;

        public Block(Vector2f position, float width, float height, Color color)
        {
            Shape = new RectangleShape(new Vector2f(width, height));
            Bounds = new Bounds(position, new Vector2f(width, height));
            Size = new Vector2f(width, height);
            Position = position;
            Color = color;
            IsStatic = true;
        }

        public override void Draw(RenderWindow window)
        {
            window.Draw(Shape);
        }

        public override void Move(float deltaTime)
        {
        }

        public override void OnCollision(GameObject obj, CollisionArgs? args)
        {
        }

        public void TakeDamage()
        {
            if (--Strength <= 0)
                Destroy();

            Color = new Color(Color.R, Color.G, Color.B, (byte)(Strength * 40));
        }

        public override void OnWallCollision(CollisionArgs? args)
        {
        }
    }
}
