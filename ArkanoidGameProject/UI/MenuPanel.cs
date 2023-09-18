using ArkanoidGameProject.UI.Components;
using ArkanoidGameProject.UI.Factory;
using SFML.Graphics;
using SFML.System;

namespace ArkanoidGameProject
{
    internal class MenuPanel : StackPanel
    {

        private readonly Button _resumeButton;
        private readonly Button _restartButton;
        private readonly Button _loadFromJsonButton;
        private readonly Button _loadFromTxtButton;
        private readonly Button _saveToJsonButton;
        private readonly Button _saveToTxtButton;
        private readonly Button _settingsButton;
        private readonly Button _exitButton;

        public event Action? Resume;
        public event Action? Restart;
        public event Action? LoadFromJson;
        public event Action? LoadFromTxt;
        public event Action? SaveToJson;
        public event Action? SaveToTxt;
        public event Action? Settings;
        public event Action? Exit;

        public MenuPanel(UIComponentsFactory factory)
        {
            _resumeButton = factory.CreateButton("Resume", new Color(255, 150, 160), new Color(255, 170, 160), Color.Black, new Vector2f(), new Vector2f(320, 50));
            _resumeButton.Click += OnResume;
            _restartButton = factory.CreateButton("Restart", new Color(255, 150, 160), new Color(255, 170, 160), Color.Black, new Vector2f(), new Vector2f(320, 50));
            _restartButton.Click += OnRestart;

            var loadButtons = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Size = new Vector2f(Size.X, 50),
                Spacing = 5,
                Padding = 0,
                BackgroundColor = Color.Transparent
            };
            _loadFromJsonButton = factory.CreateButton("Load from JSON", new Color(255, 150, 160), new Color(255, 170, 160), Color.Black, new Vector2f(), new Vector2f(160, 50));
            _loadFromJsonButton.Click += OnLoadFromJson;
            _loadFromTxtButton = factory.CreateButton("Load from TXT", new Color(255, 150, 160), new Color(255, 170, 160), Color.Black, new Vector2f(), new Vector2f(155, 50));
            _loadFromTxtButton.Click += OnLoadFromTxt;
            loadButtons.AddChild(_loadFromJsonButton);
            loadButtons.AddChild(_loadFromTxtButton);

            var saveButtons = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Size = new Vector2f(Size.X, 50),
                Spacing = 5,
                Padding = 0,
                BackgroundColor = Color.Transparent
            };
            _saveToJsonButton = factory.CreateButton("Save to JSON", new Color(255, 150, 160), new Color(255, 170, 160), Color.Black, new Vector2f(), new Vector2f(160, 50));
            _saveToJsonButton.Click += OnSaveToJson;
            _saveToTxtButton = factory.CreateButton("Save to TXT", new Color(255, 150, 160), new Color(255, 170, 160), Color.Black, new Vector2f(), new Vector2f(155, 50));
            _saveToTxtButton.Click += OnSaveToTxt;
            saveButtons.AddChild(_saveToJsonButton);
            saveButtons.AddChild(_saveToTxtButton);

            _settingsButton = factory.CreateButton("Settings", new Color(255, 150, 160), new Color(255, 170, 160), Color.Black, new Vector2f(), new Vector2f(320, 50));
            _exitButton = factory.CreateButton("Exit", new Color(255, 150, 160), new Color(255, 170, 160), Color.Black, new Vector2f(), new Vector2f(320, 50));

            AddChild(_resumeButton);
            AddChild(_restartButton);
            AddChild(loadButtons);
            AddChild(saveButtons);
            AddChild(_settingsButton);
            AddChild(_exitButton);
        }

        private void OnResume(object? sender, EventArgs e)
        {
            Resume?.Invoke();
        }

        private void OnSaveToJson(object? sender, EventArgs e)
        {
            SaveToJson?.Invoke();
        }

        private void OnSaveToTxt(object? sender, EventArgs e)
        {
            SaveToTxt?.Invoke();
        }

        private void OnLoadFromJson(object? sender, EventArgs e)
        {
            LoadFromJson?.Invoke();
        }

        private void OnLoadFromTxt(object? sender, EventArgs e)
        {
            LoadFromTxt?.Invoke();
        }

        private void OnRestart(object? sender, EventArgs e)
        {
            Restart?.Invoke();
        }

        private void OnSettings(object? sender, EventArgs e)
        {
            Settings?.Invoke();
        }

        private void OnExit(object? sender, EventArgs e)
        {
            Exit?.Invoke();
        }

        protected override void OnEnabledChanged()
        {
            base.OnEnabledChanged();

            if (IsEnabled)
                Enable();
            else
                Disable();
        }

        private void Enable()
        {
            _resumeButton.Click += OnResume;
            _restartButton.Click += OnRestart;
            _loadFromJsonButton.Click += OnLoadFromJson;
            _loadFromTxtButton.Click += OnLoadFromTxt;
            _saveToJsonButton.Click += OnSaveToJson;
            _saveToTxtButton.Click += OnSaveToTxt;
            _settingsButton.Click += OnSettings;
            _exitButton.Click += OnExit;
        }
        
        private void Disable()
        {
            _resumeButton.Click -= OnResume;
            _restartButton.Click -= OnRestart;
            _loadFromJsonButton.Click -= OnLoadFromJson;
            _loadFromTxtButton.Click -= OnLoadFromTxt;
            _saveToJsonButton.Click -= OnSaveToJson;
            _saveToTxtButton.Click -= OnSaveToTxt;
            _settingsButton.Click -= OnSettings;
            _exitButton.Click -= OnExit;
        }
    }
}
