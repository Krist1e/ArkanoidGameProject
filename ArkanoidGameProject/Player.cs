using Arkanoid.Player;
using ArkanoidGameProject.Bonuses;
using ArkanoidGameProject.GameObjects;
using ArkanoidGameProject.Input;
using SFML.System;
using SFML.Graphics;

namespace ArkanoidGameProject
{
    internal class Player : IPlayerProperties
    {
        private readonly IPlayerInput _input;
        private readonly BonusesManager _bonusesManager;
        
        private GameField _field;
        private bool _isPaused;
        
        private float _paddleMultiplier = 1f;
        private Vector2f _paddleSize = new(100, 20);
        private int _paddleSpeed;

        public Player(IPlayerInput input, IPlayerSettings settings, GameField field)
        {
            _input = input;
            _field = field;
            _bonusesManager = new BonusesManager(this);
            Balls = new Balls();
            Paddle = new Paddle(new Vector2f(field.Width / 2.0f, field.Height * 0.9f), _paddleSize.X, _paddleSize.Y, Color.Blue, PaddleSpeed);
            
            Configure(settings);
            AddObjectsToField();

            Balls.Removed += BallsOnRemoved;
            Balls.Added += BallsOnAdded;
        }
        
        public bool IsPaused
        {
            get => _isPaused;
            set
            {
                _isPaused = value;
                if (_isPaused)
                    OnPause();
                else
                    OnResume();
            }
        }

        public GameStats Stats { get; private set; } = null!;

        public IEnumerable<Bonus> ActiveBonuses => _bonusesManager.Bonuses;

        public float PaddleMultiplier
        {
            get => _paddleMultiplier;
            set
            {
                PaddleSize = PaddleSize with { X = PaddleSize.X / _paddleMultiplier };
                _paddleMultiplier = value;
                PaddleSize = PaddleSize with { X = PaddleSize.X * _paddleMultiplier };
            }
        }

        public Vector2f PaddleSize
        {
            get => _paddleSize;
            set
            {
                _paddleSize = value;
                Paddle.Size = _paddleSize;
            }
        }

        public float PaddleSpeed
        {
            get => _paddleSpeed;
            set
            {
                _paddleSpeed = (int)value;
                Paddle.Speed = _paddleSpeed;
            }
        }

        public float BallsRadius
        {
            get => Balls.Radius;
            set => Balls.Radius = value;
        }

        public int LaunchSpeed { get; set; }

        public Balls Balls { get; init; }
        public Paddle Paddle { get; private set; }
        
        public event Action<int>? ScoreChanged;
        public event Action<int>? LivesChanged; 

        private void BallsOnAdded(Ball ball) => _field.Add(ball);

        private void BallsOnRemoved(Ball ball)
        {
            _field.Remove(ball);
            if (Balls.Count == 0)
                RemoveLife();
        }
        
        public void UpdateTime(float deltaTime)
        {
            if (IsPaused) return;
            Stats.AddTime(deltaTime);
            _bonusesManager.Update(deltaTime);
        }
        
        public void Load(IPlayerSettings settings ,GameField field, GameStats? stats, IEnumerable<Bonus>? bonuses)
        {
            Configure(settings);
            if (stats != null)
            {
                Stats.Score = stats.Score;
                Stats.Time = stats.Time;
                Stats.Lives = stats.Lives >= Stats.Lives ? Stats.Lives : stats.Lives;
            }
            _field = field;
            _bonusesManager.Load(this, bonuses ?? Enumerable.Empty<Bonus>());
            Balls.Clear();
            
            if (field.GameObjects.OfType<Ball>().Any())
                foreach (var ball in field.GameObjects.OfType<Ball>())
                    Balls.Add(ball);

            if (field.GameObjects.OfType<Paddle>().Any())
            {
                Paddle = field.GameObjects.OfType<Paddle>().First();
                Paddle.Ball = Balls.First();
            }
            else
            {
                Paddle = new Paddle(new Vector2f(field.Width / 2.0f, field.Height * 0.9f), _paddleSize.X, _paddleSize.Y, Color.Blue, PaddleSpeed);
                AddObjectsToField();
            }
            
            Balls.Removed += BallsOnRemoved;
            Balls.Added += BallsOnAdded;
        }
        
        public void AddBonus(Bonus bonus) => _bonusesManager.Add(bonus);
        
        private void Configure(IPlayerSettings settings)
        {
            PaddleMultiplier = settings.DifficultyStats.PaddleMultiplier;
            PaddleSpeed = settings.DifficultyStats.PaddleSpeed;
            LaunchSpeed = settings.DifficultyStats.BallSpeed;
            Stats = new GameStats(0, settings.DifficultyStats.Lives, new TimeSpan());
        }
        
        public void AddScore(int score)
        {
            Stats.AddScore(score);
            ScoreChanged?.Invoke(Stats.Score);
        }
        
        public void AddLife()
        {
            Stats.AddLife();
            LivesChanged?.Invoke(Stats.Lives);
        }

        private void RemoveLife()
        {
            Stats.RemoveLife();
            LivesChanged?.Invoke(Stats.Lives);
            if (Stats.Lives == 0) return;
            var ball = new Ball(10, new Vector2f(), new Vector2f());
            Balls.Add(ball);
            Paddle.Ball = ball;
        }

        private void AddObjectsToField()
        {
            var ball = new Ball(10, new Vector2f(), new Vector2f());    
            
            Balls.Add(ball);
            Paddle.Ball = ball;
            _field.Add(Paddle);
            _field.Add(ball);
        }
        
        private void LaunchBall()
        {
            Paddle.ReleaseBall(LaunchSpeed);
        }

        private void OnPause()
        {
            _input.MoveRight -= Paddle.MoveRight;
            _input.MoveLeft -= Paddle.MoveLeft;
            _input.SpacePressed -= LaunchBall;
        }

        private void OnResume()
        {
            _input.MoveRight += Paddle.MoveRight;
            _input.MoveLeft += Paddle.MoveLeft;
            _input.SpacePressed += LaunchBall;
        }
    }
}