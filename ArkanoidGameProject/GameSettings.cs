using SFML.System;
using System.Numerics;

namespace ArkanoidGameProject
{
    public interface IPlayerSettings
    {
        string PlayerName { get; }
        DifficultyStats DifficultyStats { get; }
    }

    public class GameSettings : IPlayerSettings
    {
        private readonly Dictionary<DifficultyLevel, DifficultyStats> _difficultyStats = new()
        {
            [DifficultyLevel.VeryEasy] = new DifficultyStats
            {
                DifficultyLevel = DifficultyLevel.VeryEasy,
                Lives = 6,
                PaddleSpeed = 400,
                PaddleMultiplier = 2f,
                BallSpeed = 400
            },
            [DifficultyLevel.Easy] = new DifficultyStats
            {
                DifficultyLevel = DifficultyLevel.Easy,
                Lives = 5,
                PaddleSpeed = 400,
                PaddleMultiplier = 1.5f,
                BallSpeed = 500
            },
            [DifficultyLevel.Normal] = new DifficultyStats
            {
                DifficultyLevel = DifficultyLevel.Normal,
                Lives = 3,
                PaddleSpeed = 400,
                PaddleMultiplier = 1.2f,
                BallSpeed = 600
            },
            [DifficultyLevel.Hard] = new DifficultyStats
            {
                DifficultyLevel = DifficultyLevel.Hard,
                Lives = 2,
                PaddleSpeed = 400,
                PaddleMultiplier = 1f,
                BallSpeed = 700
            },
            [DifficultyLevel.VeryHard] = new DifficultyStats
            {
                DifficultyLevel = DifficultyLevel.VeryHard,
                Lives = 1,
                PaddleSpeed = 300,
                PaddleMultiplier = 0.8f,
                BallSpeed = 800
            }
        };

        public Vector2f WindowSize { get; private set; } = new(1280, 720);
        public DifficultyLevel Difficulty { get; private set; } = DifficultyLevel.Normal;
        public string PlayerName { get; set; } = "Player";
        public DifficultyStats DifficultyStats => _difficultyStats[Difficulty];

        public event Action<Vector2f>? WindowSizeChanged;
        public event Action<DifficultyStats>? DifficultyChanged;
        public event Action<string>? PlayerNameChanged;

        public void SetWindowSize(Vector2f windowSize)
        {
            if (WindowSize == windowSize)
                return;
            WindowSizeChanged?.Invoke(windowSize);
            WindowSize = windowSize;
        }

        public void SetDifficulty(DifficultyLevel difficulty)
        {
            if (Difficulty == difficulty)
                return;
            Difficulty = difficulty;
            DifficultyChanged?.Invoke(DifficultyStats);
        }

        public void SetPlayerName(string playerName)
        {
            if (PlayerName == playerName)
                return;
            PlayerName = playerName;
            PlayerNameChanged?.Invoke(PlayerName);
        }
    }
}
