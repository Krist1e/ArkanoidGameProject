using ArkanoidGameProject.UI.Components;
using ArkanoidGameProject.UI.Factory;
using SFML.System;
using SFML.Graphics;
using System.Numerics;

namespace ArkanoidGameProject.UI
{
    internal class SettingsPanel : StackPanel
    {
        private readonly TextLabel _playerNameLabel;
        private readonly TextBox _playerName;
        
        private readonly TextLabel _difficultyLabel;
        private readonly RadioButton _difficultyVeryEasy;
        private readonly RadioButton _difficultyEasy;
        private readonly RadioButton _difficultyNormal;
        private readonly RadioButton _difficultyHard;
        private readonly RadioButton _difficultyVeryHard;
        
        private readonly TextLabel _resolutionLabel;        
        private readonly RadioButton _resolutionOptionOne;
        private readonly RadioButton _resolutionOptionTwo;
        private readonly RadioButton _resolutionOptionThree;
        private readonly RadioButton _resolutionOptionFour;
        private readonly RadioButton _resolutionOptionFive;

        private readonly Button _returnButton;
        private readonly Button _applyButton;

        public event Action<string>? PlayerNameChanged;
        public event Action<DifficultyLevel>? DifficultyChanged;
        public event Action<Vector2f>? ResolutionChanged;
        public event Action? Apply;
        public event Action? Return;

        public SettingsPanel(UIComponentsFactory factory)
        {
            var player = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Size = new Vector2f(600, 50),
                Spacing = 5,
                Padding = 0,
                BackgroundColor = Color.Transparent
            };            
            _playerNameLabel = factory.CreateTextLabel("Player:", Color.Black, new Vector2f(), new Vector2f(100, 50));
            _playerNameLabel.BackgroundColor = new Color(255, 100, 110);
            _playerName = factory.CreateTextBox("Guest", Color.Black, new Color(255, 170, 160), Color.White, new Vector2f(), new Vector2f(418, 50));
            _playerName.BackgroundColor = new Color(255, 150, 160);            

            _difficultyLabel = factory.CreateTextLabel("Difficulty", Color.Black, new Vector2f(), new Vector2f(500, 50));
            _difficultyLabel.BackgroundColor = new Color(255, 100, 110);
            var difficulty = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Size = new Vector2f(Size.X, 50),
                Spacing = 5,
                Padding = 0,
                BackgroundColor = Color.Transparent
            };
            _difficultyVeryEasy = factory.CreateRadioButton("Very easy", new Color(255, 150, 160), new Color(255, 170, 160), Color.Black, new Vector2f(), new Vector2f(100, 50));
            _difficultyVeryEasy.Group = "Difficulty";
            _difficultyEasy = factory.CreateRadioButton("Easy", new Color(255, 150, 160), new Color(255, 170, 160), Color.Black, new Vector2f(), new Vector2f(100, 50));
            _difficultyEasy.Group = "Difficulty";
            _difficultyNormal = factory.CreateRadioButton("Normal", new Color(255, 150, 160), new Color(255, 170, 160), Color.Black, new Vector2f(), new Vector2f(100, 50));
            _difficultyNormal.Group = "Difficulty";
            _difficultyHard = factory.CreateRadioButton("Hard", new Color(255, 150, 160), new Color(255, 170, 160), Color.Black, new Vector2f(), new Vector2f(100, 50));
            _difficultyHard.Group = "Difficulty";
            _difficultyVeryHard = factory.CreateRadioButton("Very hard", new Color(255, 150, 160), new Color(255, 170, 160), Color.Black, new Vector2f(), new Vector2f(101, 50));
            _difficultyVeryHard.Group = "Difficulty";

            _resolutionLabel = factory.CreateTextLabel("Resolution", Color.Black, new Vector2f(), new Vector2f(600, 50));
            _resolutionLabel.BackgroundColor = new Color(255, 100, 110);
            _resolutionOptionOne = factory.CreateRadioButton("640x360", new Color(255, 150, 160), new Color(255, 170, 160), Color.Black, new Vector2f(), new Vector2f(600, 50));
            _resolutionOptionOne.Group = "Resolution";
            _resolutionOptionTwo = factory.CreateRadioButton("960x540", new Color(255, 150, 160), new Color(255, 170, 160), Color.Black, new Vector2f(), new Vector2f(600, 50));
            _resolutionOptionTwo.Group = "Resolution";
            _resolutionOptionThree = factory.CreateRadioButton("1280x720", new Color(255, 150, 160), new Color(255, 170, 160), Color.Black, new Vector2f(), new Vector2f(600, 50));
            _resolutionOptionThree.Group = "Resolution";
            _resolutionOptionFour = factory.CreateRadioButton("1600x900", new Color(255, 150, 160), new Color(255, 170, 160), Color.Black, new Vector2f(), new Vector2f(600, 50));
            _resolutionOptionFour.Group = "Resolution";
            _resolutionOptionFive = factory.CreateRadioButton("1920x1080", new Color(255, 150, 160), new Color(255, 170, 160), Color.Black, new Vector2f(), new Vector2f(600, 50));
            _resolutionOptionFive.Group = "Resolution";

            _returnButton = factory.CreateButton("Return", new Color(255, 150, 160), new Color(255, 170, 160), Color.Black, new Vector2f(), new Vector2f(258, 50));
            _returnButton.Click += OnReturn;
            _applyButton = factory.CreateButton("Apply", new Color(255, 150, 160), new Color(255, 170, 160), Color.Black, new Vector2f(), new Vector2f(258, 50));
            _applyButton.Click += OnApply;
            var actionButtons = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Size = new Vector2f(Size.X, 50),
                Spacing = 5,
                Padding = 0,
                BackgroundColor = Color.Transparent
            };

            player.AddChild(_playerNameLabel);
            player.AddChild(_playerName);
            AddChild(player);
            AddChild(_difficultyLabel);
            difficulty.AddChild(_difficultyVeryEasy);
            difficulty.AddChild(_difficultyEasy);
            difficulty.AddChild(_difficultyNormal);
            difficulty.AddChild(_difficultyHard);
            difficulty.AddChild(_difficultyVeryHard);
            AddChild(difficulty);
            AddChild(_resolutionLabel);
            AddChild(_resolutionOptionOne);
            AddChild(_resolutionOptionTwo);
            AddChild(_resolutionOptionThree);
            AddChild(_resolutionOptionFour);
            AddChild(_resolutionOptionFive);
            actionButtons.AddChild(_returnButton);
            actionButtons.AddChild(_applyButton);
            AddChild(actionButtons);
        }

        public void SetSettings(GameSettings settings)
        {
            _playerName.Content = settings.PlayerName;
            _resolutionOptionOne.IsChecked = settings.WindowSize == new Vector2f(640, 360);
            _resolutionOptionTwo.IsChecked = settings.WindowSize == new Vector2f(960, 540);
            _resolutionOptionThree.IsChecked = settings.WindowSize == new Vector2f(1280, 720);
            _resolutionOptionFour.IsChecked = settings.WindowSize == new Vector2f(1600, 900);
            _resolutionOptionFive.IsChecked = settings.WindowSize == new Vector2f(1920, 1080);
            _difficultyVeryEasy.IsChecked = settings.Difficulty == DifficultyLevel.VeryEasy;
            _difficultyEasy.IsChecked = settings.Difficulty == DifficultyLevel.Easy;
            _difficultyNormal.IsChecked = settings.Difficulty == DifficultyLevel.Normal;
            _difficultyHard.IsChecked = settings.Difficulty == DifficultyLevel.Hard;
            _difficultyVeryHard.IsChecked = settings.Difficulty == DifficultyLevel.VeryHard;
            PlayerNameChanged += settings.SetPlayerName;
            ResolutionChanged += settings.SetWindowSize;
            DifficultyChanged += settings.SetDifficulty;
        }

        protected override void OnEnabledChanged()
        {
            base.OnEnabledChanged();
            if (IsEnabled)
            {
                _applyButton.Click += OnApply;
                _returnButton.Click += OnReturn;
            } else
            {
                _applyButton.Click -= OnApply;
                _returnButton.Click -= OnReturn;
            }
        }
        private void OnApply(object? sender, EventArgs e)
        {
            PlayerNameChanged?.Invoke(_playerName.Content);

            var resolution = new Vector2f(1280, 720);
            if (_resolutionOptionOne.IsChecked)
                resolution = new Vector2f(640, 360);
            else if (_resolutionOptionTwo.IsChecked)
                resolution = new Vector2f(960, 540);
            else if (_resolutionOptionThree.IsChecked)
                resolution = new Vector2f(1280, 720);
            else if (_resolutionOptionFour.IsChecked)
                resolution = new Vector2f(1600, 900);
            else if (_resolutionOptionFive.IsChecked)
                resolution = new Vector2f(1920, 1080);
            ResolutionChanged?.Invoke(resolution);

            var difficulty = DifficultyLevel.VeryEasy;
            if (_difficultyEasy.IsChecked)
                difficulty = DifficultyLevel.Easy;
            else if (_difficultyNormal.IsChecked)
                difficulty = DifficultyLevel.Normal;
            else if (_difficultyHard.IsChecked)
                difficulty = DifficultyLevel.Hard;
            else if (_difficultyVeryHard.IsChecked)
                difficulty = DifficultyLevel.VeryHard;
            DifficultyChanged?.Invoke(difficulty);

            Apply?.Invoke();
        }

        private void OnReturn(object? sender, EventArgs e)
        {
            Return?.Invoke();
        }

        protected override void OnSizeChanged()
        {
            base.OnSizeChanged();
            _difficultyLabel.Size = new Vector2f(Size.X, 50);
            _resolutionLabel.Size = new Vector2f(Size.X, 50);
            _resolutionOptionOne.Size = new Vector2f(Size.X, 50);
            _resolutionOptionTwo.Size = new Vector2f(Size.X, 50);
            _resolutionOptionThree.Size = new Vector2f(Size.X, 50);
            _resolutionOptionFour.Size = new Vector2f(Size.X, 50);
            _resolutionOptionFive.Size = new Vector2f(Size.X, 50);
        }
    }
}
