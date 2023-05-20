using ArkanoidGameProject.GameObjects;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace ArkanoidGameProject
{
    internal class Game
    {
        public uint height;
        public uint width;
        public uint FPS { get; set; }
        public GameField field;
        public RenderWindow window;
        public float DeltaTime { get; set; }
        public Game(uint width, uint height, uint FPS)
        {
            VideoMode mode = new VideoMode(width, height);
            RenderWindow window = new RenderWindow(mode, "Arkanoid Game");
            window.SetFramerateLimit(FPS);
            this.window = window;
            this.FPS = FPS;
            field = new GameField(width, height);
            window.Closed += (sender, args) => window.Close();
            /*window.LostFocus += (sender, args) => Pause();
            window.GainedFocus += (sender, args) => Resume();*/
            this.width = width;
            this.height = height;
        }
        
        public void Pause() { }
        public void Start()
        {
            var block1 = new Block(new Vector2f(400f, 100f), 100, 50, Color.Red);
            var block2 = new Block(new Vector2f(500f, 100f), 100, 50, Color.Green);
            var block3 = new Block(new Vector2f(300f, 100f), 100, 50, Color.Cyan);
            var ball = new Ball(10, new Vector2f(width / 2, height / 2 + 190), new Vector2f(0.0f, -3.0f));
            var paddle = new Paddle(new Vector2f(width / 2, height / 2 + 200), 100, 10, Color.Blue);
            field.Add(ball);
            field.Add(block1);
            field.Add(block2);
            field.Add(block3);
            field.Add(paddle);
            while (window.IsOpen)
            {
                window.DispatchEvents();
                window.Clear();
                field.Move(1f);
                field.CheckCollision();
                field.Draw(window);
                window.Display();
            }
        }
        public void Save() { }
        public void Resume() { }
        public void Restart() { }
        public void Exit() { }
    }
}
