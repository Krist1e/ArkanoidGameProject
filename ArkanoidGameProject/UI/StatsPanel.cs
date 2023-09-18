using ArkanoidGameProject.UI.Components;
using ArkanoidGameProject.UI.Factory;
using SFML.Graphics;
using SFML.System;

namespace ArkanoidGameProject.UI
{
    internal class StatsPanel : StackPanel
    {        
        private readonly TextLabel _scoreLabel;
        private readonly TextLabel _livesLabel;
        private readonly TextLabel _playerLabel;
        private readonly TextLabel _timeLabel;
        private readonly TextLabel _difficultyLabel;
        private readonly Button _menuButton;

        public StatsPanel(UIComponentsFactory factory)
        {
            _scoreLabel = factory.CreateTextLabel("Score: ", Color.Black, new Vector2f(), new Vector2f(200, 50));
            _scoreLabel.BackgroundColor = new Color(255, 200, 160);
            _livesLabel = factory.CreateTextLabel("Lives: ", Color.Black, new Vector2f(), new Vector2f(200, 50));
            _livesLabel.BackgroundColor = new Color(255, 200, 160);
            _playerLabel = factory.CreateTextLabel("Player", Color.Black, new Vector2f(), new Vector2f(200, 50));
            _playerLabel.BackgroundColor = new Color(255, 200, 160);
            _timeLabel = factory.CreateTextLabel("Time: ", Color.Black, new Vector2f(), new Vector2f(200, 50));
            _timeLabel.BackgroundColor = new Color(255, 200, 160);
            _difficultyLabel = factory.CreateTextLabel("Difficulty: ", Color.Black, new Vector2f(), new Vector2f(240, 50));
            _difficultyLabel.BackgroundColor = new Color(255, 200, 160);
            _menuButton = factory.CreateButton("Menu", new Color(255, 150, 160), new Color(255, 170, 160), Color.Black, new Vector2f(), new Vector2f(200, 50));
            _menuButton.TextAlignment = TextAlignment.Center;
            _menuButton.Click += OnMenuButtonClicked;
            
            AddChild(_scoreLabel);
            AddChild(_livesLabel);
            AddChild(_playerLabel);
            AddChild(_timeLabel);
            AddChild(_difficultyLabel);
            AddChild(_menuButton);
        }

        public event Action? MenuButtonClicked;

        private void OnMenuButtonClicked(object? sender, EventArgs e)
        {
            MenuButtonClicked?.Invoke();
        }

        public void SetStats(GameStats stats)
        {
            SetScore(stats.Score);
            SetLives(stats.Lives);
            SetTime(stats.Time);
            stats.ScoreChanged += SetScore;
            stats.LivesChanged += SetLives;
            stats.TimeChanged += SetTime;
        }
        
        public void SetGameSettings(GameSettings settings)
        {
            SetPlayerName(settings.PlayerName);
            SetDifficulty(settings.DifficultyStats);
            settings.PlayerNameChanged += SetPlayerName;
            settings.DifficultyChanged += SetDifficulty;
        }

        public void SetPlayerName(string name)
        {
            _playerLabel.Content = name;
        }
        

        private void SetDifficulty(DifficultyStats difficulty)
        {
            _difficultyLabel.Content = difficulty.DifficultyLevel.ToString();
        }

        public void SetScore(int score)
        {
            _scoreLabel.Content = $"Score: {score}";
        }

        public void SetLives(int lives)
        {
            _livesLabel.Content = $"Lives: {lives}";
        }

        public void SetTime(TimeSpan time)
        {
            _timeLabel.Content = $"Time: {time}";
        }

        protected override void OnEnabledChanged()
        {
            base.OnEnabledChanged();
            
            if (IsEnabled)
                _menuButton.Click += OnMenuButtonClicked;
            else
                _menuButton.Click -= OnMenuButtonClicked;
        }
    }
}