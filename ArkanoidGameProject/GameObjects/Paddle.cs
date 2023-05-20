using SFML.Graphics;
using SFML.System;
using SFML.Window;


namespace ArkanoidGameProject.GameObjects
{
    internal class Paddle : GameObject
    {
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

        public new Vector2f Size
        {
            get => base.Size;
            set
            {
                base.Size = value;
                Shape.Origin = Size / 2;
                Shape.Size = Size;
            }
        }   
        public Color Color { get; set; }
        public Paddle(Vector2f position, float width, float height, Color color)
        {
            Shape = new RectangleShape(new Vector2f(width, height));
            Size = new Vector2f(width, height);
            Position = position;
            Color = color;            
            Shape.Position = Position;
            Shape.FillColor = Color;            
            IsStatic = true;
        }

        public override void Draw(RenderWindow window)
        {
            window.Draw(Shape);
        }

        public void MoveLeft(float deltaTime)
        {
            Position -= new Vector2f(10 * deltaTime, 0);
            Shape.Position = Position;
        }

        public void MoveRight(float deltaTime)
        {
            Position += new Vector2f(10 * deltaTime, 0);
            Shape.Position = Position;
        }

        public override void Move(float deltaTime)
        {
            if (Keyboard.IsKeyPressed(Keyboard.Key.Left))
            {
                this.MoveLeft(deltaTime);
            } 
            else if (Keyboard.IsKeyPressed(Keyboard.Key.Right))
            {
                this.MoveRight(deltaTime);
            }             
        }

        public override void OnCollision(GameObject obj, CollisionArgs? args)
        {
            
        }

        public override void OnWallCollision(CollisionArgs? args)
        {
        }
    }
}
