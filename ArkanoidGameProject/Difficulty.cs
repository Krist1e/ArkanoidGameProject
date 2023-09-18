namespace ArkanoidGameProject
{
    public enum DifficultyLevel
    {
        VeryEasy,
        Easy,
        Normal,
        Hard,
        VeryHard
    }

    public readonly struct DifficultyStats
    {
        public DifficultyStats()
        {
        }

        public DifficultyLevel DifficultyLevel { get; init; }
        public int Lives { get; init; }
        public int PaddleSpeed { get; init; }
        public float PaddleMultiplier { get; init; }
        public int BallSpeed { get; init; }
    }
}